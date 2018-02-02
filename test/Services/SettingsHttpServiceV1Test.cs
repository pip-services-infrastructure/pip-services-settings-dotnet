using Moq;
using PipServices.Commons.Config;
using PipServices.Commons.Convert;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Settings.Data.Version1;
using PipServices.Settings.Logic;
using PipServices.Settings.Persistence;
using PipServices.Settings.Services.Version1;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PipServices.Settings.Services
{
    public class SettingsHttpServiceV1Test : IDisposable
    {
        private static SettingSectionV1 SETTING1 = CreateSetting("1", new ConfigParams());
        private static SettingSectionV1 SETTING2 = CreateSetting("2", new ConfigParams(new Dictionary<string, string>(){
                    { "param", "0"}
                }));

        private SettingsMemoryPersistence _persistence;
        private SettingsController _controller;
        private SettingsHttpServiceV1 _service;

        public SettingsHttpServiceV1Test()
        {
            _persistence = new SettingsMemoryPersistence();
            _controller = new SettingsController();

            var config = ConfigParams.FromTuples(
                "connection.protocol", "http",
                "connection.host", "localhost",
                "connection.port", "3000"
            );
            _service = new SettingsHttpServiceV1();
            _service.Configure(config);

            var references = References.FromTuples(
                new Descriptor("pip-services-settings", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("pip-services-settings", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("pip-services-settings", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);
            _service.SetReferences(references);
            //_service.OpenAsync(null).Wait();

            // Todo: This is defect! Open shall not block the tread
            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

        public void Dispose()
        {
            _service.CloseAsync(null).Wait();
        }

        private static SettingSectionV1 CreateSetting(string id, ConfigParams p)
        {
            SettingSectionV1 setting = new SettingSectionV1();
            setting.Id = id;
            setting.Parameters = p;
            return setting;
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create one setting
            ConfigParams param = await Invoke<ConfigParams>("/settings/set_section", new { id = SETTING1.Id, parameters = SETTING1.Parameters  });

            Assert.NotNull(param);
            Assert.Equal(SETTING1.Parameters, param);

            // Create another setting
            param = await Invoke<ConfigParams>("/settings/set_section", new { id = SETTING2.Id, parameters = SETTING2.Parameters });

            Assert.NotNull(param);
            Assert.Equal(SETTING2.Parameters, param);

            // Get all settings
            DataPage<SettingSectionV1> page = await Invoke<DataPage<SettingSectionV1>>("/settings/get_sections", new { });
            Assert.NotNull(page);
            Assert.NotNull(page.Data);
            Assert.Equal(2, page.Data.Count);

            //Get all sections ids 
            List<string> idsActual = new List<string>();
            idsActual.Add(SETTING1.Id);
            idsActual.Add(SETTING2.Id);
            
            DataPage<string> ids = await Invoke<DataPage<string>>("/settings/get_section_ids", new { });
            Assert.NotNull(ids);
            Assert.NotNull(ids.Data);
            Assert.Equal(2, page.Data.Count);
            Assert.Equal(idsActual, ids.Data);

            
            // Update the setting
            ConfigParams updateParams = new ConfigParams();
            updateParams["newKey"] = "text";
            param = await Invoke<ConfigParams>("/settings/modify_section", new { id = SETTING1.Id, update_parameters = updateParams });
           
            Assert.NotNull(param);
            Assert.Equal(updateParams, param);
            
            updateParams = new ConfigParams();
            updateParams["param"] = "5";
            param = await Invoke<ConfigParams>("/settings/modify_section", new { id = SETTING2.Id, increment_parameters = updateParams });

            Assert.NotNull(param);
            Assert.Equal(updateParams, param);

            // Try to get deleted setting
            SettingSectionV1 setting = await Invoke<SettingSectionV1>("/settings/delete_setting_by_id", new { id = SETTING2.Id });
            Assert.Null(setting);
        }

        private static async Task<T> Invoke<T>(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestValue = JsonConverter.ToJson(request);
                using (var content = new StringContent(requestValue, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("http://localhost:3000" + route, content);
                    var responseValue = response.Content.ReadAsStringAsync().Result;
                    return JsonConverter.FromJson<T>(responseValue);
                }
            }
        }
    }


    }

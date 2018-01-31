using Moq;
using PipServices.Commons.Config;
using PipServices.Commons.Convert;
using PipServices.Commons.Refer;
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
    public class SettingsHttpServiceV1Test : AbstractTest
    {
        private SettingsHttpServiceV1 _service;

        private ISettingsController _settingsController;
        private ISettingsPersistence _settingsPersistence;

        private Mock<ISettingsController> _moqSettingsController;
        private Mock<ISettingsPersistence> _moqSettingsPersistence;

        ConfigParams _restConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", 3001 // another port for this test!
        );

        private TestModel Model { get; set; }

        protected override void Initialize()
        {
            Model = new TestModel();

            _moqSettingsController = new Mock<ISettingsController>();
            _settingsController = _moqSettingsController.Object;

            _moqSettingsController.Setup(c => c.GetCommandSet()).Returns(new SettingsCommandSet(_settingsController));

            _moqSettingsPersistence = new Mock<ISettingsPersistence>();
            _settingsPersistence = _moqSettingsPersistence.Object;

            _service = new SettingsHttpServiceV1();
            _service.Configure(_restConfig);

            var references = References.FromTuples(
                new Descriptor("pip-services-settings", "persistence", "memory", "default", "1.0"), _settingsPersistence,
                new Descriptor("pip-services-settings", "controller", "default", "default", "1.0"), _settingsController,
                new Descriptor("pip-services-settings", "service", "http", "default", "1.0"), _service
            );
            _service.SetReferences(references);

            Task.Run(() => _service.OpenAsync(Model.CorrelationId));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

        protected override void Uninitialize()
        {
            _service.CloseAsync(null);
        }

        [Fact] // Just ONE test to avoid issues with re-opening service on the same host
        public void It_Should_Perform_CRUD_Operations()
        {
            It_Should_Be_Opened();

            It_Should_Create_Setting_Async();

            It_Should_Update_Setting_Async();

            It_Should_Delete_Setting_Async();

            It_Should_Get_Setting_Async();

            It_Should_Get_Settings_Async();
        }

        public void It_Should_Be_Opened()
        {
            Assert.True(_service.IsOpened());
        }

        public void It_Should_Create_Setting_Async()
        {
            var createCalled = false;
            _moqSettingsController.Setup(c => c.SetSectionAsync(Model.CorrelationId, Model.SampleSetting1.Id, Model.SampleSetting1.Parameters)).Callback(() => createCalled = true);

            SendPostRequest("create_setting", new
            {
                correlation_id = Model.CorrelationId,
                setting = Model.SampleSetting1
            });

            Assert.True(createCalled);
        }

        public void It_Should_Update_Setting_Async()
        {
            var updateCalled = false;
            _moqSettingsController.Setup(c => c.ModifySectionAsync(Model.CorrelationId, Model.SampleSetting1.Id, Model.SampleSetting1.Parameters, null)).Callback(() => updateCalled = true);

            SendPostRequest("update_setting", new
            {
                correlation_id = Model.CorrelationId,
                setting = Model.SampleSetting1
            });

            Assert.True(updateCalled);
        }

        public void It_Should_Delete_Setting_Async()
        {
            var deleteCalled = false;
            _moqSettingsController.Setup(c => c.DeleteSectionByIdAsync(Model.CorrelationId, Model.SampleSetting1.Id)).Callback(() => deleteCalled = true);

            SendPostRequest("delete_setting_by_id", new
            {
                correlation_id = Model.CorrelationId,
                setting_id = Model.SampleSetting1.Id
            });

            Assert.True(deleteCalled);
        }

        public void It_Should_Get_Setting_Async()
        {
            var getCalled = false;
            _moqSettingsController.Setup(c => c.GetSectionByIdAsync(Model.CorrelationId, Model.SampleSetting1.Id)).Callback(() => getCalled = true);

            SendPostRequest("get_setting_by_id", new
            {
                correlation_id = Model.CorrelationId,
                setting_id = Model.SampleSetting1.Id
            });

            Assert.True(getCalled);
        }

      

        public void It_Should_Get_Settings_Async()
        {
            var getCalled = false;
            _moqSettingsController.Setup(c => c.GetSectionsAsync(Model.CorrelationId, Model.FilterParams, Model.PagingParams)).Callback(() => getCalled = true);

            SendPostRequest("get_settings", new
            {
                correlation_id = Model.CorrelationId,
                filter = Model.FilterParams,
                paging = Model.PagingParams
            });

            Assert.True(getCalled);
        }

        private static string SendPostRequest(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                using (var content = new StringContent(JsonConverter.ToJson(request), Encoding.UTF8, "application/json"))
                {
                    var response = httpClient.PostAsync("http://localhost:3001/settings/" + route, content).Result;

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}

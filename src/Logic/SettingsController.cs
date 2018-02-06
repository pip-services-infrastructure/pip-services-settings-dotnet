using PipServices.Commons.Commands;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Settings.Data.Version1;
using System.Linq;
using PipServices.Settings.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipServices.Settings.Logic
{
    public class SettingsController : ISettingsController
    {
        private static ConfigParams _defaultConfig = ConfigParams.FromTuples(
            "dependencies.persistence", "pip-services-settings:persistence:*:*:1.0"
        );

        private DependencyResolver _dependencyResolver = new DependencyResolver(SettingsController._defaultConfig);
        private ISettingsPersistence _persistence;
        private SettingsCommandSet _commandSet;
        

        public SettingsController()
        {
            _commandSet = new SettingsCommandSet(this);
        }

        public void SetReferences(IReferences references)
        {
            _dependencyResolver.SetReferences(references);
            _persistence = _dependencyResolver.GetOneRequired<ISettingsPersistence>("persistence");
        }

        public CommandSet GetCommandSet()
        {
            return _commandSet ?? (_commandSet = new SettingsCommandSet(this));
        }

        public async Task<DataPage<string>> GetSectionIdsAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            DataPage<SettingSectionV1> page = await _persistence.GetPageByFilterAsync(correlationId, filter, paging);
            if (page != null)
            {

                List<string> data = page.Data.Select(d => d.Id).ToList<string>();
                var result = new DataPage<string>(data, page.Total);
                return result;
            }
            return null;   
        }

        public async Task<DataPage<SettingSectionV1>> GetSectionsAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await _persistence.GetPageByFilterAsync(correlationId, filter, paging);
        }

        public async Task<Dictionary<string, dynamic>> GetSectionByIdAsync(string correlationId, string id)
        {
            SettingSectionV1 item = await _persistence.GetOneByIdAsync(correlationId, id);

            Dictionary<string, dynamic> parameters = item != null ? item.Parameters : null;
            parameters = parameters != null ? parameters : new Dictionary<string, dynamic>();

            return parameters;

        }

        public async Task<Dictionary<string, dynamic>> SetSectionAsync(string correlationId, string id, Dictionary<string, dynamic> parameters)
        {
            SettingSectionV1 item = new SettingSectionV1(id, parameters);
            Console.WriteLine("try to create with ID: " + item.Id);
            Console.WriteLine("_persistence: " + _persistence.);
            SettingSectionV1 settings = await _persistence.SetAsync(correlationId, item);
            Console.WriteLine("try to set to persistence with ID: " + settings.Id);
            return settings.Parameters;
        }

        public async Task<Dictionary<string, dynamic>> ModifySectionAsync(string correlationId, string id, Dictionary<string, dynamic> updateParams, Dictionary<string, dynamic> incrementParams)
        {
            SettingSectionV1 settings = await _persistence.ModifyAsync(correlationId, id, updateParams, incrementParams);
            return settings.Parameters;
        }

        public Task<SettingSectionV1> DeleteSectionByIdAsync(string correlationId, string id)
        {
            return _persistence.DeleteByIdAsync(correlationId, id);
        }

    }
}
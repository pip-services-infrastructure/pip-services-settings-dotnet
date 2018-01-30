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
            DataPage<SettingParamsV1> page = await _persistence.GetPageByFilterAsync(correlationId, filter, paging);
            if (page != null)
            {

                List<string> data = page.Data.Select(d => d.Id).ToList<string>();
                var result = new DataPage<string>(data, page.Total);
                return result;
            }
            return null;   
        }

        public async Task<DataPage<SettingParamsV1>> GetSectionsAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await _persistence.GetPageByFilterAsync(correlationId, filter, paging);
        }

        public async Task<ConfigParams> GetSectionByIdAsync(string correlationId, string id)
        {
            SettingParamsV1 item = await _persistence.GetOneByIdAsync(correlationId, id);

            ConfigParams parameters = item != null ? item.parameters : null;
            parameters = parameters != null ? parameters : new ConfigParams();

            return parameters;

        }

        public async Task<ConfigParams> SetSectionAsync(string correlationId, string id, ConfigParams parameters)
        {
            SettingParamsV1 item = new SettingParamsV1(id, parameters);
            SettingParamsV1 settings = await _persistence.SetAsync(correlationId, item);
            return settings.parameters;
        }

        public async Task<ConfigParams> ModifySectionAsync(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams)
        {
            SettingParamsV1 settings = await _persistence.ModifyAsync(correlationId, id, updateParams, incrementParams);
            return settings.parameters;
        }

        
    }
}
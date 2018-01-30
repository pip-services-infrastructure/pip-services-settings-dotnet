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

        public delegate void SectionIdsDelegat(Object err, DataPage<string> page);
        public delegate void SectionsDelegat(Object err, DataPage<SettingParamsV1> page);
        public delegate void SectionDelegat(Object err, ConfigParams parameters);

        public SettingsController()
        {
            _commandSet = new SettingsCommandSet(this);
        }

        public void setReferences(IReferences references)
        {
            _dependencyResolver.SetReferences(references);
            _persistence = _dependencyResolver.GetOneRequired<ISettingsPersistence>("persistence");
        }

        public CommandSet GetCommandSet()
        {
            return _commandSet ?? (_commandSet = new SettingsCommandSet(this));
        }

        public Task<DataPage<string>> getSectionIds(string correlationId, FilterParams filter, PagingParams paging)
        {
            Task<DataPage<SettingParamsV1>> page = _persistence.GetPageByFilter(correlationId, filter, paging);
            page.Wait();
            if (page.Result != null)
            {

                List<string> data = page.Result.Data.Select(d => d.Id).ToList<string>();
                var result = new DataPage<string>(data, page.Result.Total);
                return Task.FromResult(result);
            }
            return Task.FromResult(new DataPage<string>());   
        }

        public Task<DataPage<SettingParamsV1>> getSections(string correlationId, FilterParams filter, PagingParams paging)
        {
            return _persistence.GetPageByFilter(correlationId, filter, paging);
        }

        public Task<ConfigParams> getSectionById(string correlationId, string id)
        {
            Task<SettingParamsV1> item = _persistence.GetOneById(correlationId, id);
            item.Wait();
            //if (item.) callback(err, null);

            ConfigParams parameters = item != null ? item.Result.parameters : null;
            parameters = parameters != null ? parameters : new ConfigParams();

            return Task.FromResult(parameters);

        }

        public Task<ConfigParams> setSection(string correlationId, string id, ConfigParams parameters)
        {
            SettingParamsV1 item = new SettingParamsV1(id, parameters);
            Task<SettingParamsV1> settings = _persistence.Set(correlationId, item);
            return Task.FromResult(_persistence.Set(correlationId, item).Result.parameters);
        }

        public Task<ConfigParams> modifySection(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams)
        {
            return Task.FromResult(_persistence.Modify(correlationId, id, updateParams, incrementParams).Result.parameters);
        }

        /*
        public void getSectionIds(string correlationId, FilterParams filter, PagingParams paging, SectionIdsDelegat callback)
        {
            Task<DataPage<SettingParamsV1>> page =  _persistence.GetPageByFilter(correlationId, filter, paging);
            page.Wait();
            if (page.Result != null)
            {
               
                List<string> data =  page.Result.Data.Select(d => d.Id).ToList<string>();
                var result = new DataPage<string>(data, page.Result.Total);
                callback(new Object(), result);
            }
        }

        public void getSections(string correlationId, FilterParams filter, PagingParams paging, SectionsDelegat callback)
        {
            Task<DataPage<SettingParamsV1>> page = _persistence.GetPageByFilter(correlationId, filter, paging);
            page.Wait();
            callback(null, page.Result);
        }

        public void getSectionById(string correlationId, string id, SectionDelegat callback)
        {
            Task<SettingParamsV1> item = _persistence.GetOneById(correlationId, id);
            item.Wait();
                //if (item.) callback(err, null);
    
            ConfigParams parameters = item != null ? item.Result.parameters : null;
                parameters =  parameters != null ? parameters : new ConfigParams();
            callback(null, parameters);
            
        }

        public void setSection(string correlationId, string id, ConfigParams parameters, SectionDelegat callback)
        {
            SettingParamsV1 item = new SettingParamsV1(id, parameters);
            Task<SettingParamsV1> settings = _persistence.Set(correlationId, item);
            settings.Wait();
            callback(null, settings.Result.parameters);
        }


        public void modifySection(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams, SectionDelegat callback)
        {
            Task<SettingParamsV1> settings = _persistence.Modify(correlationId, id, updateParams, incrementParams);
            settings.Wait();
            callback(null, settings.Result.parameters);
        }*/
    }
}
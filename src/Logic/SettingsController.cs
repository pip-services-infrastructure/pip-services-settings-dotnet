using PipServices.Commons.Commands;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Settings.Data.Version1;
using PipServices.Settings.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipServices.Settings.Logic
{
    class SettingsController : ISettingsController
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

        public void setReferences(IReferences references)
        {
            _dependencyResolver.SetReferences(references);
            _persistence = _dependencyResolver.GetOneRequired<ISettingsPersistence>("persistence");
        }

        public SettingsCommandSet getCommandSet()
        {
            if (_commandSet == null)
                _commandSet = new SettingsCommandSet(this);
            return _commandSet;
        }

        public void getSectionIds(string correlationId, FilterParams filter, PagingParams paging,
           (Object err, DataPage<string> page) callback)
        {
            _persistence.getPageByFilter(correlationId, filter, paging, (err, page) =>
            {
                if (page != null)
                {
                    let data = _.map(page.data, d => d.id);
                    let result = new DataPage<string>(data, page.total);
                    callback(err, result);
                }
                else
                {
                    callback(err, null);
                }
            });
        }

        public void getSections(string correlationId, FilterParams filter, PagingParams paging,
            (Object err, DataPage<SettingParamsV1> page) callback)
        {
            _persistence.getPageByFilter(correlationId, filter, paging, callback);
        }

        public void getSectionById(string correlationId, string id,
            (Object err, ConfigParams parameters) callback)
        {
            _persistence.getOneById(correlationId, id, (err, item) =>
            {
                if (err) callback(err, null);
                else
                {
                    let parameters = item != null ? item.parameters : null;
                    parameters = parameters || new ConfigParams();
                    callback(null, parameters);
                }
            });
        }

        public void setSection(string correlationId, string id, ConfigParams parameters,
            (Object err, ConfigParams parameters) callback)
        {
            SettingParamsV1 item = new SettingParamsV1(id, parameters);
            _persistence.set(correlationId, item, (err, itemBack) =>
            {
                if (callback)
                {
                    if (err) callback(err, null);
                    else callback(null, itemBack.parameters);
                }
            });
        }

        public void modifySection(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams,
           (Object err, ConfigParams parameters) callback)
        {
            _persistence.modify(correlationId, id, updateParams, incrementParams, (err, item) =>
            {
                if (callback)
                {
                    if (err) callback(err, null);
                    else callback(null, item.parameters);
                }
            });
        }
    }
}
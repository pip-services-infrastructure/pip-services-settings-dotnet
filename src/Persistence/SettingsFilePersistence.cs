using PipServices.Commons.Config;
using PipServices.Data.File;
using PipServices.Settings.Data.Version1;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices.Settings.Persistence
{
    class SettingsFilePersistence : SettingsMemoryPersistence
    {
        protected JsonFilePersister<SettingParamsV1> _persister;

        public SettingsFilePersistence()
        {
            _persister = new JsonFilePersister<SettingParamsV1>();
            _loader = _persister;
            _saver = _persister;
        }

        public override void Configure(ConfigParams config)
        {
            base.Configure(config);

            _persister.Configure(config);
        }
    }
}

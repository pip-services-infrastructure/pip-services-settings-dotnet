using System;
using System.Collections.Generic;
using System.Text;
using PipServices.Container;
using PipServices.Settings.Build;

namespace PipServices.Settings.Container
{
    public class SettingsProcess : ProcessContainer
    {
        public SettingsProcess()
            : base("settings", "Settings microservice")
        {
            _factories.Add(new SettingsServiceFactory());
        }
    }
}

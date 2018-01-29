using PipServices.Commons.Config;
using PipServices.Commons.Data;
using System;
using System.Collections.Generic;

namespace PipServices.Settings.Data.Version1
{
    public class SettingParamsV1 : IStringIdentifiable
    {
        public SettingParamsV1() { }
            
        public SettingParamsV1(string id, ConfigParams param)
        {
            this.Id = id;
            this.parameters = param;
            this.update_time = new DateTime();
        }

        public SettingParamsV1(string id)
        {
            this.Id = id;
            this.parameters = new ConfigParams();
            this.update_time = new DateTime();
        }

        public string Id { get; set; }
        public ConfigParams parameters { set; get; }
        public DateTime update_time { set; get; }

    }
}

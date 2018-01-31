using PipServices.Commons.Config;
using PipServices.Commons.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PipServices.Settings.Data.Version1
{
    [DataContract]
    public class SettingParamsV1 : IStringIdentifiable
    {
        public SettingParamsV1() { }
            
        public SettingParamsV1(string id, ConfigParams param)
        {
            this.Id = id;
            this.Parameters = param;
            this.UpdateTime = new DateTime();
        }

        public SettingParamsV1(string id)
        {
            this.Id = id;
            this.Parameters = new ConfigParams();
            this.UpdateTime = new DateTime();
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "parameters")]
        public ConfigParams Parameters { set; get; }
        [DataMember(Name = "update_time")]
        public DateTime UpdateTime { set; get; }

    }
}

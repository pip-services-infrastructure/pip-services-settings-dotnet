using PipServices.Commons.Config;
using PipServices.Commons.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PipServices.Settings.Data.Version1
{
    [DataContract]
    public class SettingSectionV1 : IStringIdentifiable
    {
        public SettingSectionV1() { }
            
        public SettingSectionV1(string id, Dictionary<string, dynamic> param)
        {
            this.Id = id;
            this.Parameters = param;
            this.UpdateTime = new DateTime();
        }

        public SettingSectionV1(string id)
        {
            this.Id = id;
            this.Parameters = new Dictionary<string, dynamic>();
            this.UpdateTime = new DateTime();
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "parameters")]
        public Dictionary<string, dynamic> Parameters { set; get; }
        [DataMember(Name = "update_time")]
        public DateTime UpdateTime { set; get; }
    }
}

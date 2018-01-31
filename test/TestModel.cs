using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Settings.Data.Version1;
using System;
using System.Collections.Generic;

namespace PipServices.Settings
{
    class TestModel
    {
        public string CorrelationId { get; set; }

        public SettingParamsV1 SampleSetting1 { get; set; }
        public SettingParamsV1 SampleSetting2 { get; set; }

        public FilterParams FilterParams { get; set; }
        public PagingParams PagingParams { get; set; }

        public TestModel()
        {
            CorrelationId = "1";

            SampleSetting1 = new SettingParamsV1("1", new ConfigParams(new Dictionary<string, string>(){
                    { "param1 ", "Test English Setting"},
                    { "param2", "Test Spanish Citar"},
                    { "param3", "0"},
                }));
            SampleSetting2 = new SettingParamsV1("2", new ConfigParams(new Dictionary<string, string>(){
                    { "param1 ", "Test English Setting for settings with id = 2"},
                    { "param2", "Test Spanish Citar for settings with id = 2"},
                }));


            FilterParams = new FilterParams();
            PagingParams = new PagingParams();
        }
    }
}

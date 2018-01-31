using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Random;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices.Settings.Data.Version1
{
    public static class RandomSettingParamsV1
    {
        public static ConfigParams ConfigParams()
        {
            int count = RandomInteger.NextInteger(0, 5);
            ConfigParams config = new ConfigParams();

            for (int index = 0; index < count; index++)
                config[RandomText.Word().ToLower()] = RandomText.Words(1, 20).ToLower();

            return config;
        }

        public static SettingParamsV1 SettingParams()
        {
            
            return new SettingParamsV1
            {
                Id = IdGenerator.NextLong(),
                Parameters = ConfigParams(),
                UpdateTime = new DateTime()
            };
        }
    }
}

using PipServices.Commons.Config;
using PipServices.Commons.Validate;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices.Settings.Data.Version1
{
    public class SettingsV1Schema : ObjectSchema
    {
        public SettingsV1Schema()
        {
            WithOptionalProperty("Id", TypeCode.String);
            WithOptionalProperty("UpdateTime", TypeCode.DateTime);
            WithOptionalProperty("Parameters", null); // Config Params 
        }
    }
}

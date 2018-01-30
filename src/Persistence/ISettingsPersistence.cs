using PipServices.Settings.Data.Version1;
using PipServices.Commons.Data;
using PipServices.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PipServices.Commons.Config;

namespace PipServices.Settings.Persistence
{
    interface ISettingsPersistence
    {
        Task<DataPage<SettingParamsV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging);

        Task<SettingParamsV1> GetOneByIdAsync(string correlationId, string id);
    
        Task<SettingParamsV1> SetAsync(string correlationId, SettingParamsV1 item);

        Task<SettingParamsV1> ModifyAsync(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams);
    }
}

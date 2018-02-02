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
    public interface ISettingsPersistence : IGetter<SettingSectionV1, string>, IWriter<SettingSectionV1, string>
    {
        Task<DataPage<SettingSectionV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging);

        Task<SettingSectionV1> GetOneByIdAsync(string correlationId, string id);
    
        Task<SettingSectionV1> SetAsync(string correlationId, SettingSectionV1 item);

        Task<SettingSectionV1> ModifyAsync(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams);
    }
}

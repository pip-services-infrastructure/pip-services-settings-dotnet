using PipServices.Commons.Commands;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Settings.Data.Version1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipServices.Settings.Logic
{
    public interface ISettingsController
    {
        CommandSet GetCommandSet();
        Task<DataPage<string>> GetSectionIdsAsync(string correlationId, FilterParams filter, PagingParams paging);
        Task<DataPage<SettingParamsV1>> GetSectionsAsync(string correlationId, FilterParams filter, PagingParams paging);
        Task<ConfigParams> GetSectionByIdAsync(string correlationId, string id);
        Task<ConfigParams> SetSectionAsync(string correlationId, string id, ConfigParams parameters);
        Task<ConfigParams> ModifySectionAsync(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams);
    }
}

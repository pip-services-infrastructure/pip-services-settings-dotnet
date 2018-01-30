using PipServices.Commons.Commands;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Settings.Data.Version1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static PipServices.Settings.Logic.SettingsController;

namespace PipServices.Settings.Logic
{
    public interface ISettingsController
    {


        CommandSet GetCommandSet();
       /* void getSectionIds(string correlationId, FilterParams filter, PagingParams paging, SectionIdsDelegat callback);

        void getSections(string correlationId, FilterParams filter, PagingParams paging, SectionsDelegat callback);

        void getSectionById(string correlationId, string id, SectionDelegat callback);

        void setSection(string correlationId, string id, ConfigParams parameters, SectionDelegat callback);

        void modifySection(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams, SectionDelegat callback);

        */
        
        Task<DataPage<string>> getSectionIds(string correlationId, FilterParams filter, PagingParams paging);
        Task<DataPage<SettingParamsV1>> getSections(string correlationId, FilterParams filter, PagingParams paging);
        Task<ConfigParams> getSectionById(string correlationId, string id);
        Task<ConfigParams> setSection(string correlationId, string id, ConfigParams parameters);
        Task<ConfigParams> modifySection(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams);
       
       */
    }
}

using PipServices.Commons.Commands;
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

        void getSectionIds(string correlationId, FilterParams filter, PagingParams paging,
       (Object err, DataPage<string> page) callback);

        /*
    getSections(correlationId: string, filter: FilterParams, paging: PagingParams,
        callback: (err: any, page: DataPage<SettingsSectionV1>) => void): void;
    
    getSectionById(correlationId: string, id: string,
        callback: (err: any, parameters: ConfigParams) => void): void;

    setSection(correlationId: string, id: string, parameters: ConfigParams,
        callback?: (err: any, parameters: ConfigParams) => void): void;

    modifySection(correlationId: string, id: string, updateParams: ConfigParams, incrementParams: ConfigParams,
        callback?: (err: any, parameters: ConfigParams) => void): void;
    }*/
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Data.MongoDb;
using PipServices.Settings.Data.Version1;

namespace PipServices.Settings.Persistence
{
    class SettingsMongoDbPersistence : IdentifiableMongoDbPersistence<SettingParamsV1, string>, ISettingsPersistence
    {
        public SettingsMongoDbPersistence() : base("settings") { }

        private FilterDefinition<SettingParamsV1> ComposeFilter(FilterParams filterParams)
        {
            filterParams = filterParams ?? new FilterParams();
            var search = filterParams.GetAsNullableString("search");

            var id = filterParams.GetAsNullableString("id");


            var builder = Builders<SettingParamsV1>.Filter;
            var filter = builder.Empty;

            if (id != null) filter &= builder.Eq(q => q.Id, id);

            return filter;
        }

        private static Object mapToPublic(Array map)
        {

            return map;
        }

        private static string fieldFromPublic(string field)
        {
            if (field == null) return null;
            field = field.Replace(".", "_dot_");
            return field;
        }

        private static Object mapFromPublic(Object map)
        {
            if (map == null) return null;

            return map;
        }


        public Task<SettingParamsV1> GetOneById(string correlationId, string id)
        {
            return base.GetOneByIdAsync(correlationId, id);
        }

        public Task<DataPage<SettingParamsV1>> GetPageByFilter(string correlationId, FilterParams filter, PagingParams paging)
        {
            return base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

        public Task<SettingParamsV1> Modify(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams)
        {
            throw new NotImplementedException();
        }

        public Task<SettingParamsV1> Set(string correlationId, SettingParamsV1 item)
        {
            throw new NotImplementedException();
        }
    }
}

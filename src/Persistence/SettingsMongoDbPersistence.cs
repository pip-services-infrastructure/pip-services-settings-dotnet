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
    public class SettingsMongoDbPersistence : IdentifiableMongoDbPersistence<SettingParamsV1, string>, ISettingsPersistence
    {
        public SettingsMongoDbPersistence() : base("settings") { }

        private FilterDefinition<SettingParamsV1> ComposeFilter(FilterParams filterParams)
        {
            filterParams = filterParams ?? new FilterParams();
            string search = filterParams.GetAsNullableString("search");

            string id = filterParams.GetAsNullableString("id");


            var builder = Builders<SettingParamsV1>.Filter;
            var filter = builder.Empty;

            if (id != null) filter &= builder.Eq(q => q.Id, id);

            return filter;
        }

        
        public async Task<SettingParamsV1> GetOneByIdAsync(string correlationId, string id)
        {
            return await base.GetOneByIdAsync(correlationId, id);
        }

        public async Task<DataPage<SettingParamsV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

        public async Task<SettingParamsV1> ModifyAsync(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams)
        {

            SettingParamsV1 item = new SettingParamsV1(id);
            item.UpdateTime = new DateTime();

            // Update parameters
            if (updateParams != null)
            {
                foreach (var key in updateParams)
                {
                   item.Parameters[key.Key] = key.Value;
                }
            }

            // Increment parameters
            if (incrementParams != null)
            {
                foreach (var key in incrementParams)
                {
                    long increment = Convert.ToInt64(key.Value);
                    long value = Convert.ToInt64(item.Parameters[key.Key]);
                    value += increment;
                    item.Parameters[key.Key] = value.ToString();
                    
                }
            }

            return await base.UpdateAsync(correlationId, item);
        }

        public async Task<SettingParamsV1> SetAsync(string correlationId, SettingParamsV1 item)
        {
            item.UpdateTime = new DateTime();
            return await base.SetAsync(correlationId, item);
        }
    }
}

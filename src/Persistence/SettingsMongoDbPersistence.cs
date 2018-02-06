using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Data.MongoDb;
using PipServices.Settings.Data.Version1;

namespace PipServices.Settings.Persistence
{
    public class SettingsMongoDbPersistence : IdentifiableMongoDbPersistence<SettingSectionV1, string>, ISettingsPersistence
    {
        public SettingsMongoDbPersistence() : base("settings") { }

        private FilterDefinition<SettingSectionV1> ComposeFilter(FilterParams filterParams)
        {
            filterParams = filterParams ?? new FilterParams();
            string search = filterParams.GetAsNullableString("search");

            string id = filterParams.GetAsNullableString("id");


            var builder = Builders<SettingSectionV1>.Filter;
            var filter = builder.Empty;

            if (id != null) filter &= builder.Eq(q => q.Id, id);

            return filter;
        }

        
        public async Task<SettingSectionV1> GetOneByIdAsync(string correlationId, string id)
        {
            return await base.GetOneByIdAsync(correlationId, id);
        }

        public async Task<DataPage<SettingSectionV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

        public async Task<SettingSectionV1> ModifyAsync(string correlationId, string id, Dictionary<string, dynamic> updateParams, Dictionary<string, dynamic> incrementParams)
        {

            SettingSectionV1 item = new SettingSectionV1(id);
            item.UpdateTime = new DateTime();

            // Update parameters
            if (updateParams != null)
            {
                foreach (var key in updateParams)
                {
                    item.Parameters[key.Key] = key.Value;
                }
                this._collection.FindOneAndUpdate<SettingSectionV1>(e => e.Id == id, Builders<SettingSectionV1>.Update.Set(e => e.Parameters, item.Parameters));

                return await GetOneByIdAsync(correlationId, id);
            }
            else {
                var keys = incrementParams.Keys;
                var update = getIncUpdate(incrementParams);
                
                if (update != null) this._collection.FindOneAndUpdate<SettingSectionV1>(e => e.Id == id, update);

                return await GetOneByIdAsync(correlationId, id);
            }
        }

        private UpdateDefinition<SettingSectionV1> getIncUpdate(Dictionary<string, dynamic> incrementParams) {
            if (incrementParams.Count == 0) return null;

            var list = incrementParams.ToList();
            var update = Builders<SettingSectionV1>.Update.Inc("Parameters." + list[0].Key, Convert.ToInt64(list[0].Value));
            var i = 1;
            while (list.Count > i) {
                update = update.Inc("Parameters." + list[i].Key, Convert.ToInt64(list[i].Value));
                i++;
            }

            return update;

        }

        public async Task<SettingSectionV1> SetAsync(string correlationId, SettingSectionV1 item)
        {
            item.UpdateTime = new DateTime();
            return await base.SetAsync(correlationId, item);
        }
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using PipServices.Data.Memory;
using PipServices.Settings.Data.Version1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using PipServices.Commons.Config;
using PipServices.Commons.Data;

namespace PipServices.Settings.Persistence
{
    public class SettingsMemoryPersistence : IdentifiableMemoryPersistence<SettingSectionV1, string>, ISettingsPersistence
    {

        public SettingsMemoryPersistence() : base() { }

        private bool MatchSearch(SettingSectionV1 item, string search)
        {
            return (item.Id != null && item.Id.Contains(search)) ? true : false;
        }

        public int ItemsCount { get { return _items.Count; } }

        private IList<Func<SettingSectionV1, bool>> ComposeFilter(FilterParams filter)
        {
            var result = new List<Func<SettingSectionV1, bool>>();

            filter = filter ?? new FilterParams();

            var search = filter.GetAsNullableString("search");
            var id = filter.GetAsNullableString("id");

            result.Add(setting => string.IsNullOrWhiteSpace(search) || MatchSearch(setting, search));
            result.Add(setting => string.IsNullOrWhiteSpace(id) || setting.Id.Equals(id));

            return result;
        }

        public async Task<DataPage<SettingSectionV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

        public async Task<SettingSectionV1> GetOneByIdAsync(string correlationId, string id)
        {
            FilterParams filter = new FilterParams();
            filter.Add("id", id);

            return await base.GetOneRandomAsync(correlationId, ComposeFilter(filter));
        }

        public async Task<SettingSectionV1> SetAync(string correlationId, SettingSectionV1 item)
        {
            item.UpdateTime = new DateTime();

            return await base.SetAsync(correlationId, item);
        }

        public async Task<SettingSectionV1> ModifyAsync(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams)
        {

            int index = this._items.FindIndex(x => x.Id == id);

            SettingSectionV1 item = index >= 0
                ? this._items[index] : new SettingSectionV1(id);

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

            return item;
        }
    }

}

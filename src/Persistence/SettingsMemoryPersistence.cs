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
    public class SettingsMemoryPersistence : IdentifiableMemoryPersistence<SettingParamsV1, string>, ISettingsPersistence
    {

        public SettingsMemoryPersistence() : base() { }

        private bool MatchSearch(SettingParamsV1 item, string search)
        {
            return (item.Id != null && item.Id.Contains(search)) ? true : false;
        }

        private IList<Func<SettingParamsV1, bool>> ComposeFilter(FilterParams filter)
        {
            var result = new List<Func<SettingParamsV1, bool>>();

            filter = filter ?? new FilterParams();

            var search = filter.GetAsNullableString("search");
            var id = filter.GetAsNullableString("id");

            result.Add(setting => string.IsNullOrWhiteSpace(search) || MatchSearch(setting, search));
            result.Add(setting => string.IsNullOrWhiteSpace(id) || setting.Id.Equals(id));

            return result;
        }

        public Task<DataPage<SettingParamsV1>> GetPageByFilter(string correlationId, FilterParams filter, PagingParams paging)
        {
            return base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

        public Task<SettingParamsV1> GetOneById(string correlationId, string id)
        {
            FilterParams filter = new FilterParams();
            filter.Add("id", id);

            return base.GetOneRandomAsync(correlationId, ComposeFilter(filter));
        }

        public Task<SettingParamsV1> Set(string correlationId, SettingParamsV1 item)
        {
            item.update_time = new DateTime();

            return base.SetAsync(correlationId, item);
        }

        public Task<SettingParamsV1> Modify(string correlationId, string id, ConfigParams updateParams, ConfigParams incrementParams)
        {

            int index = this._items.FindIndex(x => x.Id == id);

            SettingParamsV1 item = index >= 0
                ? this._items[index] : new SettingParamsV1(id);

            // Update parameters
            if (updateParams != null)
            {
                foreach (var key in updateParams)
                {
                    if (updateParams.GetType().GetProperty(key.Key) != null)
                        item.parameters[key.Key] = updateParams[key.Value];
                }
            }

            // Increment parameters
            if (incrementParams != null)
            {
                foreach (var key in incrementParams)
                {
                    if (incrementParams.GetType().GetProperty(key.Key) != null)
                    {
                        long increment = Convert.ToInt64(incrementParams[key.Key], 0);
                        long value = Convert.ToInt64(item.parameters[key.Key], 0);
                        value += increment;
                        item.parameters[key.Key] = value.ToString();
                    }
                }
            }

            return Task.FromResult(item);
        }
    }

}

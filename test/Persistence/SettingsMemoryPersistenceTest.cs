using PipServices.Commons.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PipServices.Settings.Persistence
{
    public class SettingsMemoryPersistenceTest 
    {
        private SettingsMemoryPersistence _persistence;
        private SettingsPersistenceFixture _fixture;

        public SettingsMemoryPersistenceTest()
        {
            _persistence = new SettingsMemoryPersistence();

            _persistence.OpenAsync(null).Wait();
            _persistence.ClearAsync(null).Wait();

            _fixture = new SettingsPersistenceFixture(_persistence);
        }
        
        [Fact]
        public async Task TestMemoryCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        [Fact]
        public async Task TestMemoryGetByFilterAsync()
        {
            await _fixture.TestGetByFilterAsync();
        }
    }
}

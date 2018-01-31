using PipServices.Commons.Config;
using PipServices.Commons.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PipServices.Settings.Persistence
{
    public class SettingsMongoDbPersistenceTest : IDisposable
    {
        private SettingsMongoDbPersistence _persistence;
        private SettingsPersistenceFixture _fixture;

        public SettingsMongoDbPersistenceTest()
        {
            var MONGODB_COLLECTION = Environment.GetEnvironmentVariable("MONGODB_COLLECTION") ?? "test_quotes";
            var MONGODB_SERVICE_URI = Environment.GetEnvironmentVariable("MONGODB_SERVICE_URI") ?? "mongodb://localhost:27017/test";

            var config = ConfigParams.FromTuples(
                "collection", MONGODB_COLLECTION,
                "connection.uri", MONGODB_SERVICE_URI
            );

            _persistence = new SettingsMongoDbPersistence();
            _persistence.Configure(config);
            _persistence.OpenAsync(null).Wait();
            _persistence.ClearAsync(null).Wait();

            _fixture = new SettingsPersistenceFixture(_persistence);
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestMongoDbCrudOperationsAsync()
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

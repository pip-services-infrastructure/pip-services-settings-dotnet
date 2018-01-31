using PipServices.Commons.Config;
using PipServices.Commons.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PipServices.Settings.Persistence
{
    public class SettingsMongoDbPersistenceTest : AbstractTest
    {
        private TestModel Model { get; set; }
        private SettingsMongoDbPersistence settingsPersistence;

        public SettingsMongoDbPersistenceTest()
        {
        }

        protected override void Initialize()
        {
            Model = new TestModel();

            //var config = YamlConfigReader.ReadConfig(null, "./config/test_connections.yaml", null);
            //var dbConfig = config.GetSection("mongodb");

            var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI");
            var mongoHost = Environment.GetEnvironmentVariable("MONGO_HOST") ?? "localhost";
            var mongoPort = Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017";
            var mongoDatabase = Environment.GetEnvironmentVariable("MONGO_DB") ?? "test";

            if (mongoUri == null && mongoHost == null)
                return;

            var dbConfig = ConfigParams.FromTuples(
                "connection.uri", mongoUri,
                "connection.host", mongoHost,
                "connection.port", mongoPort,
                "connection.database", mongoDatabase
            );

            settingsPersistence = new SettingsMongoDbPersistence();
            settingsPersistence.Configure(dbConfig);

            settingsPersistence.OpenAsync(null).Wait();
            settingsPersistence.ClearAsync(null).Wait();
        }

        protected override void Uninitialize()
        {
        }

       
        //[Fact]
        public void It_Should_Create_Async()
        {
            settingsPersistence.CreateAsync(Model.CorrelationId, Model.SampleSetting1).Wait();
            var setting = settingsPersistence.GetOneByIdAsync(Model.CorrelationId, Model.SampleSetting1.Id).Result;
            Assert.Equal(Model.SampleSetting1, setting);
        }

        //[Fact]
        public void It_Should_Get_Page_Async_By_Search_Filter()
        {
            var filter = new FilterParams
            {
                { "search", "test" }
            };

            CreateTestSettings(settingsPersistence);

            var result = settingsPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(4, result.Data.Count);
        }
     

       
       // [Fact]
        public void It_Should_Get_Page_Async_By_Null_Search_Filter()
        {
            var filter = new FilterParams
            {
                { "search", string.Empty }
            };

            CreateTestSettings(settingsPersistence);

            var result = settingsPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(4, result.Data.Count);
        }

        private void CreateTestSettings(ISettingsPersistence settingsPersistence)
        {
            settingsPersistence.SetAsync(Model.CorrelationId, Model.SampleSetting1).Wait();
            settingsPersistence.SetAsync(Model.CorrelationId, Model.SampleSetting2).Wait();
        }
    }
}

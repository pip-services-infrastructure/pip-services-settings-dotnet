using PipServices.Commons.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PipServices.Settings.Persistence
{
    public class SettingsMemoryPersistenceTest : AbstractTest
    {
        private TestModel Model { get; set; }

        protected override void Initialize()
        {
            Model = new TestModel();
        }

        protected override void Uninitialize()
        {
        }

        [Fact]
        public void It_Should_Set_Async()
        {
            var settingsMemoryPersistence = new SettingsMemoryPersistence();

            settingsMemoryPersistence.SetAsync(Model.CorrelationId, Model.SampleSetting1).Wait();

            Assert.Equal(1, settingsMemoryPersistence.ItemsCount);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Search_Filter()
        {
            var settingsMemoryPersistence = new SettingsMemoryPersistence();

            var filter = new FilterParams
            {
                { "search", "test" }
            };

            CreateTestSettings(settingsMemoryPersistence);

            var result = settingsMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(settingsMemoryPersistence.ItemsCount, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Null_Search_Filter()
        {
            var settingsMemoryPersistence = new SettingsMemoryPersistence();

            var filter = new FilterParams
            {
                { "search", string.Empty }
            };

            CreateTestSettings(settingsMemoryPersistence);

            var result = settingsMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(4, result.Data.Count);
        }

        private void CreateTestSettings(ISettingsPersistence settingsPersistence)
        {
            settingsPersistence.SetAsync(Model.CorrelationId, Model.SampleSetting1).Wait();
            settingsPersistence.SetAsync(Model.CorrelationId, Model.SampleSetting2).Wait();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PipServices.Commons.Refer;
using PipServices.Settings;
using PipServices.Settings.Persistence;

using PipServices.Commons.Data;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PipServices.Settings.Data.Version1;

namespace PipServices.Settings.Logic
{
    public class SettingsControllerTest : AbstractTest
    {
        private SettingsController _settingsController;

        private ISettingsPersistence _settingsPersistence;
        private Mock<ISettingsPersistence> _moqSettingsPersistence;

        private TestModel Model { get; set; }

        protected override void Initialize()
        {
            Model = new TestModel();

            var references = new References();
            _settingsController = new SettingsController();

            _moqSettingsPersistence = new Mock<ISettingsPersistence>();
            _settingsPersistence = _moqSettingsPersistence.Object;

            references.Put(new Descriptor("pip-services-settings", "persistence", "memory", "default", "1.0"), _settingsPersistence);
            references.Put(new Descriptor("pip-services-settings", "controller", "default", "default", "1.0"), _settingsController);

            _settingsController.SetReferences(references);
        }

        protected override void Uninitialize()
        {
        }
        
        [Fact]
        public void It_Should_Create_Setting_Async()
        {
            var createCalled = false;
            _moqSettingsPersistence.Setup(p => p.SetAsync(Model.CorrelationId, Model.SampleSetting1)).Callback(() => createCalled = true);

            _settingsController.SetSectionAsync(Model.CorrelationId, Model.SampleSetting1.Id, Model.SampleSetting1.Parameters);

            Assert.True(createCalled);
        }

        [Fact]
        public void It_Should_Update_Setting_Async()
        {
            var updateCalled = false;
            _moqSettingsPersistence.Setup(p => p.ModifyAsync(Model.CorrelationId, Model.SampleSetting2.Id, Model.SampleSetting1.Parameters, null)).Callback(() => updateCalled = true);

            _settingsController.ModifySectionAsync(Model.CorrelationId, Model.SampleSetting2.Id, Model.SampleSetting1.Parameters, null);

            Assert.True(updateCalled);
        }


        [Fact]
        public void It_Should_Get_Settings_Async()
        {
            var initialDataPage = new DataPage<SettingParamsV1>()
            {
                Data = new List<SettingParamsV1>() { Model.SampleSetting1, Model.SampleSetting2 },
                Total = 2
            };

            _moqSettingsPersistence.Setup(p => p.GetPageByFilterAsync(Model.CorrelationId, null, null)).Returns(Task.FromResult(initialDataPage));

            var resultDataPage = _settingsController.GetSectionsAsync(Model.CorrelationId, null, null).Result;
            Assert.Equal(initialDataPage.Data.Count, resultDataPage.Data.Count);
            Assert.Equal(initialDataPage.Total, resultDataPage.Total);
        }

        [Fact]
        public void It_Should_Get_One_Setting_Async()
        {
            var id = Model.SampleSetting2.Id;
            _moqSettingsPersistence.Setup(p => p.GetOneByIdAsync(Model.CorrelationId, id)).Returns(Task.FromResult(Model.SampleSetting2));

            var resultSetting = _settingsController.GetSectionByIdAsync(Model.CorrelationId, id).Result;
            Assert.Equal(Model.SampleSetting2.Parameters, resultSetting);
        }

        [Fact]
        public void It_Should_Delete_Section_Async()
        {
            var deleteCalled = false;
            _moqSettingsPersistence.Setup(p => p.DeleteByIdAsync(Model.CorrelationId, Model.SampleSetting1.Id)).Callback(() => deleteCalled = true);

            _settingsController.DeleteSectionByIdAsync(Model.CorrelationId, Model.SampleSetting1.Id);

            Assert.True(deleteCalled);
        }

    }
}

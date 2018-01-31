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
using PipServices.Commons.Config;

namespace PipServices.Settings.Logic
{
    public class SettingsControllerTest {
        private static SettingParamsV1 SETTING1 = CreateSetting("1", new ConfigParams());
        private static SettingParamsV1 SETTING2 = new SettingParamsV1("2", new ConfigParams(new Dictionary<string, string>(){
                    { "param", "0"}
                }));

        private  SettingsMemoryPersistence _persistence;
    private  SettingsController _controller;

    public  SettingsControllerTest()
    {
        _persistence = new  SettingsMemoryPersistence();
        _controller = new  SettingsController();

        var references = References.FromTuples(
            new Descriptor("pip-services-settings", "persistence", "memory", "default", "1.0"), _persistence
        );
        _controller.SetReferences(references);
    }

    private static  SettingParamsV1 CreateSetting(string id, ConfigParams p)
    {
        SettingParamsV1 setting = new SettingParamsV1();
            setting.Id = id;
            setting.Parameters = p;
        return setting;
    }

    [Fact]
    public async Task TestCrudOperationsAsync()
    {
        // Create one setting
        SettingParamsV1 setting1 = await _persistence.SetAsync(null, SETTING1);

       Assert.NotNull(setting1);
       Assert.Equal(SETTING1.Id, setting1.Id);

        // Create another setting
        SettingParamsV1 setting2 = await _persistence.SetAsync(null, SETTING2);

        Assert.NotNull(setting2);
        Assert.Equal(SETTING2.Id, setting2.Id);

        // Get all settings
        DataPage< SettingParamsV1> page = await _persistence.GetPageByFilterAsync(null, null, null);
        Assert.NotNull(page);
        Assert.NotNull(page.Data);
        Assert.Equal(2, page.Data.Count);

        // Update the setting
        ConfigParams param = new ConfigParams();
            param["newKey"] = "text";
         SettingParamsV1 setting = await _persistence.ModifyAsync(
            null,
            setting1.Id,
            param,
            null
        );

        Assert.NotNull(setting);
        Assert.Equal(setting1.Id, setting.Id);
        Assert.Equal(param, setting.Parameters);

        param = new ConfigParams();
            param["param"] = "5";
           setting = await _persistence.ModifyAsync(
               null,
               setting2.Id,
               null,
               param
           );

            Assert.NotNull(setting);
            Assert.Equal(setting2.Id, setting.Id);
            Assert.Equal(param, setting.Parameters);

            // Delete the setting
            await _persistence.DeleteByIdAsync(null, setting1.Id);

        // Try to get deleted setting
        setting = await _persistence.GetOneByIdAsync(null, setting1.Id);
        Assert.Null(setting);
    }

}
}

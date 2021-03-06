﻿using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Settings.Data.Version1;
using PipServices.Settings.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PipServices.Settings.Persistence
{
    public class SettingsPersistenceFixture
    {
        private SettingSectionV1 SETTING1 = new SettingSectionV1("1", new Dictionary<string, dynamic>());
        private SettingSectionV1 SETTING2 = new SettingSectionV1("2", new Dictionary<string, dynamic>(){
                    { "param", 2}
                });

        private ISettingsPersistence _persistence;

        public SettingsPersistenceFixture(ISettingsPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create one setting
            SettingSectionV1 setting1 = await _persistence.SetAsync(null, SETTING1);

            Assert.NotNull(setting1);
            Assert.Equal(SETTING1.Id, setting1.Id);

            // Create another setting
            SettingSectionV1 setting2 = await _persistence.SetAsync(null, SETTING2);

            Assert.NotNull(setting2);
            Assert.Equal(SETTING2.Id, setting2.Id);

            // Get all settings
            DataPage<SettingSectionV1> page = await _persistence.GetPageByFilterAsync(null, null, null);
            Assert.NotNull(page);
            Assert.NotNull(page.Data);
            Assert.Equal(2, page.Data.Count);

            //Get all sections ids 
            List<string> idsActual = new List<string>();
            idsActual.Add(SETTING1.Id);
            idsActual.Add(SETTING2.Id);

            // Update the setting
            Dictionary<string, dynamic> parameters = new Dictionary<string, dynamic>();
            parameters["newKey"] = "text";
            SettingSectionV1 settings = await _persistence.ModifyAsync(
                null,
                setting1.Id,
                parameters,
                null
            );
            
            Assert.NotNull(settings);
            Assert.Equal(setting1.Id, settings.Id);
            Assert.Equal(parameters, settings.Parameters);

            parameters = new Dictionary<string, dynamic>();
            parameters["param"] = 5;
            SettingSectionV1 modify2 = await _persistence.ModifyAsync(
                null,
                setting2.Id,
                null,
                parameters
            );

            Assert.NotNull(settings);
            Assert.Equal(setting2.Id, modify2.Id);
            Assert.Equal(7, modify2.Parameters["param"]);

            // Delete the setting
            await _persistence.DeleteByIdAsync(null, setting1.Id);

            // Try to get deleted setting
            settings = await _persistence.GetOneByIdAsync(null, setting1.Id);
            Assert.Null(settings);
        }

        public async Task TestGetByFilterAsync()
        {
            // Create items
            await _persistence.CreateAsync(null, SETTING1);
            await _persistence.CreateAsync(null, SETTING2);

            // Get by id
            FilterParams filter = FilterParams.FromTuples("id", "1");
            DataPage<SettingSectionV1> page = await _persistence.GetPageByFilterAsync(null, filter, null);
            Assert.Single(page.Data);
        }
    }
}

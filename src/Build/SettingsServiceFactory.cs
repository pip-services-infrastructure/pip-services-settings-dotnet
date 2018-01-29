using System;
using System.Collections.Generic;
using System.Text;

using PipServices.Commons.Build;
using PipServices.Commons.Refer;
using PipServices.Settings.Logic;
using PipServices.Settings.Persistence;
using PipServices.Settings.Services.Version1;

namespace PipServices.Settings.Build
{
    class SettingsServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("pip-services-settings", "factory", "default", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("pip-services-settings", "persistence", "memory", "*", "1.0");
        public static Descriptor FilePersistenceDescriptor = new Descriptor("pip-services-settings", "persistence", "file", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("pip-services-quotes", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("pip-services-settings", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("pip-services-settings", "service", "http", "*", "1.0");

        public SettingsServiceFactory()
        {
            RegisterAsType(MemoryPersistenceDescriptor, typeof(SettingsMemoryPersistence));
            RegisterAsType(FilePersistenceDescriptor, typeof(SettingsFilePersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(SettingsMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(SettingsController));
            RegisterAsType(HttpServiceDescriptor, typeof(SettingsHttpServiceV1));
        }
    }
}

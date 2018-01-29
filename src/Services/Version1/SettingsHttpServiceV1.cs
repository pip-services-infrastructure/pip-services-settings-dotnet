using PipServices.Commons.Refer;
using PipServices.Net.Rest;

namespace PipServices.Settings.Services.Version1
{
    public class SettingsHttpServiceV1 : CommandableHttpService
    {
        public SettingsHttpServiceV1()
            : base("settings")
        {
            _dependencyResolver.Put("controller", new Descriptor("pip-services-settings", "controller", "default", "*", "1.0"));
        }
    }
}
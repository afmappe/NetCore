using Microsoft.Extensions.Configuration;

namespace NetCore.Test.Helpers
{
    public static class TestHelper
    {
        public static AppSettings GetApplicationConfiguration()
        {
            AppSettings configuration = new AppSettings();
            IConfigurationRoot iConfig = GetIConfigurationRoot();
            iConfig.GetSection("AppSettings")
                .Bind(configuration);

            return configuration;
        }

        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
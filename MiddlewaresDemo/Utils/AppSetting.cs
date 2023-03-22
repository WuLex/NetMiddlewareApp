namespace MiddlewaresDemo.Utils
{
    public static class AppSetting
    {
        private static readonly IConfiguration Config;

        static AppSetting()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Config = builder.Build();
        }

        public static string GetConfig(string name)
        {
            return Config[name];
        }
    }
}
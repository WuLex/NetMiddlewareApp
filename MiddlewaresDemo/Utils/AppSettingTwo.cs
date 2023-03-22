namespace MiddlewaresDemo.Utils
{
    public class AppSettingTwo
    {
        private static readonly object objLock = new object();
        private static AppSettingTwo instance = null;

        private IConfigurationRoot Config { get; }

        private AppSettingTwo()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Config = builder.Build();
        }

        public static AppSettingTwo GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new AppSettingTwo();
                    }
                }
            }

            return instance;
        }

        public static string GetConfig(string name)
        {
            return GetInstance().Config.GetSection(name).Value;
            //使用方法：
            //string CONNECTION_STRING = AppSetting.GetConfig("ConnectionStrings:AdventureWorksDb");
            //或者

            //return GetInstance().Config.GetConnectionString(dbname);
            //使用方法：
            //string CONNECTION_STRING = AppSetting.GetConfig("AdventureWorksDb");
        }
    }
}
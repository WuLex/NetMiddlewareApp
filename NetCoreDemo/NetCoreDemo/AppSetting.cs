using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreDemo
{
    public class AppSetting
    {
        private static readonly object objLock = new object();
        private static AppSetting instance = null;

        private IConfigurationRoot Config { get; }

        private AppSetting()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Config = builder.Build();
        }

        public static AppSetting GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new AppSetting();
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

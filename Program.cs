using BaoCron.Services;
using Base.Models;
using Base.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BaoCron
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //1.initial & load BaoCronConfig.json
            IConfiguration configBuild = new ConfigurationBuilder()
                .AddJsonFile("BaoCronConfig.json", optional: true, reloadOnChange: true)
                .Build();

            //2.appSettings "FunConfig" section -> _Fun.Config
            var config = new ConfigDto();
            configBuild.GetSection("FunConfig").Bind(config);
            _Fun.Config = config;

            //3.setup our DI
            var services = new ServiceCollection();

            //4.base user info for base component
            services.AddSingleton<IBaseUserService, BaseUserService>();

            //5.ado.net for mssql
            services.AddTransient<DbConnection, SqlConnection>();
            services.AddTransient<DbCommand, SqlCommand>();

            //6.initial _Fun by mssql
            IServiceProvider diBox = services.BuildServiceProvider();
            _Fun.Init(false, diBox);

            //7.run main 
            await new MyService().RunAsync();
        }
    }
}

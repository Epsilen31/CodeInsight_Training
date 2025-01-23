using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Repository;
using Codeinsight.StreamingManagementSystem.Settings;
using Microsoft.Extensions.Configuration;

namespace Codeinsight.StreamingManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            string configFilePath =
                @"E:\C#\Assignment\abhishek-mishra-training-2025\Codeinsight.StreamingManagementSystem\AppSetting.json";

            IConfiguration config = new ConfigurationBuilder().AddJsonFile(configFilePath).Build();

            IConfigurationSection section = config.GetSection("AppSetting");

            AppSetting appSetting =
                section.Get<AppSetting>()
                ?? throw new InvalidOperationException("AppSetting section is missing or invalid.");

            DatabaseConnection databaseConnection = DatabaseConnection.GetInstance(appSetting);

            databaseConnection.Connect();

            IUnitOfWork unitOfWork = new UnitOfWork(databaseConnection);

            databaseConnection.Disconnect();
        }
    }
}

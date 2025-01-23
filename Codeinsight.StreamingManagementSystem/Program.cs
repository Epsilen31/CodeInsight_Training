using Microsoft.Extensions.Configuration;

namespace Codeinsight.StreamingManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            string configFilePath = "AppSettings.json";
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(configFilePath)
            .Build();
            IConfigurationSection section = config.GetSection("AppSetting");
            DatabaseSettings dbSettings = JsonSerializer.Deserialize<DatabaseSettings>(section);
            var databaseService = new DatabaseConnection(dbSettings);
            databaseService.Connect();

            IWorkOfUnit workOfUnit = new WorkOfUnit(dbSettings);

        }
    }
}
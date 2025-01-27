using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Repository;
using Codeinsight.StreamingManagementSystem.Settings;
using Microsoft.Extensions.Configuration;

namespace Codeinsight.StreamingManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var appSettings = GetAppSetting();

                var unitOfWork = new UnitOfWork(appSettings);

                var billingAndSubscriptionManager = new BillingAndSubscriptionManager(unitOfWork);
                billingAndSubscriptionManager.ManageSubscriptionAndBilling();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        private static AppSetting GetAppSetting()
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "AppSetting.json");

            var config = new ConfigurationBuilder().AddJsonFile(configFilePath).Build();

            var section = config.GetSection("AppSetting");

            return section.Get<AppSetting>()
                ?? throw new InvalidOperationException("AppSetting section is missing or invalid.");
        }
    }
}

using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.Core.Setting;
using Microsoft.Extensions.Configuration;

namespace Codeinsight.StreamingManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AppSetting appSetting = GetAppSetting();

                IBillingAndSubscriptionManager billingAndSubscriptionManager =
                    new BillingAndSubscriptionManager(appSetting);

                billingAndSubscriptionManager.ManageSubscriptionAndBilling();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        private static AppSetting GetAppSetting()
        {
            string configFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "AppSetting.json"
            );

            var config = new ConfigurationBuilder().AddJsonFile(configFilePath).Build();

            var section = config.GetSection("AppSetting");

            return section.Get<AppSetting>()
                ?? throw new InvalidOperationException("AppSetting section is missing or invalid.");
        }
    }
}

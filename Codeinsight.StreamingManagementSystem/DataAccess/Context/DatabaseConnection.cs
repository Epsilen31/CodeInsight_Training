namespace Codeinsight.StreamingManagementSystem.DataAccess.Context
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;
        private static DatabaseConnection _instance;

        private DatabaseConnection(AppSetting appSetting)
        {
            _connectionString = $"Server={appSetting.Server};Port={appSetting.Port};Database={appSetting.Database};User Id={appSetting.Username};Password={appSetting.Password};";
        }

        public static DatabaseConnection GetInstance(AppSetting appSetting)
        {
            _instance = new DatabaseConnection(appSetting);
            return _instance;
        }

        public void Connect()
        {
            Console.WriteLine("Connecting to database...");
            Console.WriteLine($"Connection String: {_connectionString}");
            Console.WriteLine("Connected to database successfully.");
        }
    }
}

#nullable enable
using Codeinsight.StreamingManagementSystem.Settings;
using MySql.Data.MySqlClient;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Context
{
    public class DatabaseConnection : IDisposable
    {
        private readonly string _connectionString;
        private static DatabaseConnection? _instance;
        private MySqlConnection? _connection;

        private DatabaseConnection(AppSetting appSetting)
        {
            _connectionString =
                $"Server={appSetting.Server};Database={appSetting.Database};User Id={appSetting.UserID};Password={appSetting.Password};";
        }

        public static DatabaseConnection GetInstance(AppSetting appSetting)
        {
            if (_instance == null)
            {
                _instance = new DatabaseConnection(appSetting);
            }

            return _instance;
        }

        public void Connect()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while disconnecting: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}

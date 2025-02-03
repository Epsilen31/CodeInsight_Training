using System.Data;
using Codeinsight.StreamingManagementSystem.Core.Setting;
using MySql.Data.MySqlClient;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Context
{
    public class DatabaseConnection : IDisposable
    {
        private readonly string _connectionString;
        private static DatabaseConnection _instance;
        private MySqlConnection _connection;

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

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection(_connectionString);
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
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

using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;
using Dapper;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection _context;

        public UserRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public ICollection<User> GetAllUsers()
        {
            using var connection = _context.Connection;
            string query = "SELECT * FROM Users";
            return connection.Query<User>(query).AsList();
        }

        public User GetUserById(int id)
        {
            using var connection = _context.Connection;
            string query = "SELECT * FROM Users WHERE Id = @Id";
            return connection.QuerySingleOrDefault<User>(query, new { Id = id });
        }

        public void AddUser(User user)
        {
            using var connection = _context.Connection;
            string query = "INSERT INTO Users (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";
            connection.Execute(query, user);
        }

        public void UpdateUser(User user)
        {
            using var connection = _context.Connection;
            string query =
                "UPDATE Users SET Name = @Name, Email = @Email , Phone = @Phone WHERE Id = @Id";
            connection.Execute(query, user);
        }

        public void DeleteUser(int id)
        {
            using var connection = _context.Connection;
            string query = "DELETE FROM Users WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }
    }
}

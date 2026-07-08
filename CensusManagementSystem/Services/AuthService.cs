using System;
using System.Data;
using Microsoft.Data.SqlClient;
using CensusManagementSystem.DataAccess;
using CensusManagementSystem.Helpers;
using CensusManagementSystem.Models;

namespace CensusManagementSystem.Services
{
    public class AuthService
    {
        public User Login(string username, string password)
        {
            string query = "SELECT UserId, Username, PasswordHash, FullName, Role, CreatedAt, IsActive FROM Users WHERE Username = @Username AND IsActive = 1";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count == 0)
                return null;

            string storedHash = dt.Rows[0]["PasswordHash"].ToString();
            if (!PasswordHelper.VerifyPassword(password, storedHash))
                return null;

            return new User
            {
                UserId = Convert.ToInt32(dt.Rows[0]["UserId"]),
                Username = dt.Rows[0]["Username"].ToString(),
                FullName = dt.Rows[0]["FullName"].ToString(),
                Role = dt.Rows[0]["Role"].ToString(),
                CreatedAt = Convert.ToDateTime(dt.Rows[0]["CreatedAt"]),
                IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"])
            };
        }

        public bool Register(string username, string password, string fullName, string role = "Officer")
        {
            if (IsUsernameExists(username))
                return false;

            string query = "INSERT INTO Users (Username, PasswordHash, FullName, Role) VALUES (@Username, @PasswordHash, @FullName, @Role)";
            string hash = PasswordHelper.HashPassword(password);
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@PasswordHash", hash),
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@Role", role)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool IsUsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }
    }
}

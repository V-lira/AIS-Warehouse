using AIS.Warehouse.Data.Models;
using AIS.Warehouse.UI.Models;
using System;
using System.Data.SqlClient;

namespace AIS.Warehouse.Logic
{
    public class AuthService
    {
        private readonly string _connectionString =
            @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";

        private readonly LogService _logService;
        public AuthService()
        {
            _logService = new LogService();
        }
        public UserSession Login(string email, string password, UserRole role)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"SELECT Id, FirstName, LastName, Login, PasswordHash, Role
                        FROM Users 
                        WHERE Login = @Login AND PasswordHash = @PasswordHash AND Role = @Role";
                    var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Login", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.AddWithValue("@Role", role.ToString());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["Id"]);
                            string userName = reader["FirstName"].ToString();
                            _logService.Log( userId,$"Успешный вход в систему. Роль: {role}","Auth",null);
                            return new UserSession
                            {
                                UserId = userId,
                                FirstName = userName,
                                Role = role
                            };
                        }
                        else
                        {
                            _logService.Log(0,$"Неудачная попытка входа. Логин: {email}, Роль: {role}","AuthError",null);
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Log(0,$"Ошибка при аутентификации: {ex.Message}","AuthError", null);
                return null;
            }
        }
    }
}
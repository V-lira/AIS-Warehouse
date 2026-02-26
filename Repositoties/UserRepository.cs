using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data;

namespace AIS.Warehouse.Repositoties
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public UserDto GetByEmail(string email)
        {
            SqlConnection connection = DbConnectionFactory.Create();
            connection.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT Id, FirstName, LastName, Initials, Login, PasswordHash, Role " +
                "FROM dbo.Users " +
                "WHERE Login = @login",
                connection);

            cmd.Parameters.AddWithValue("@login", email);

            SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                reader.Close();
                connection.Close();
                return null;
            }

            UserDto user = new UserDto
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Initials = reader.GetString(3),
                Login = reader.GetString(4),
                PasswordHash = reader.GetString(5),
                Role = reader.GetString(6)
            };

            reader.Close();
            connection.Close();

            return user;
        }
        public List<UserDto> GetAllEmployees()
        {
            var employees = new List<UserDto>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT Id, FirstName, LastName, Initials, Login, Role " +
                    "FROM Users " +
                    "WHERE Role = 'Employee' " +
                    "ORDER BY LastName, FirstName",
                    connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new UserDto
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Initials = reader.GetString(3),
                        Login = reader.GetString(4),
                        Role = reader.GetString(5)
                    });
                }

                reader.Close();
            }

            return employees;
        }
        public int GetEmployeesCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM Users 
                    WHERE Role = 'Employee'
                ", connection);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
using AIS.Warehouse.Data.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class EmployeeRepository2
    {
        private readonly string _connectionString = DbConnectionFactory.ConnectionString;

        public List<UserDto> GetAll()
        {
            var list = new List<UserDto>();

            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                SELECT Id, FirstName, LastName, Initials, Login, Role, DateOfRegistration
                FROM Users
                WHERE Role='Employee'
                ORDER BY LastName, FirstName", connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public List<UserDto> Search(string text)
        {
            var list = new List<UserDto>();

            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                SELECT Id, FirstName, LastName, Initials, Login, Role, DateOfRegistration
                FROM Users
                WHERE Role='Employee' AND
                      (FirstName LIKE @t OR LastName LIKE @t OR Login LIKE @t)
                ORDER BY LastName, FirstName", connection);

            cmd.Parameters.AddWithValue("@t", $"%{text}%");

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public void Add(UserDto user)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Users (FirstName, LastName, Initials, Login, PasswordHash, Role)
                VALUES (@fn, @ln, @init, @login, @pass, 'Employee')", connection);

            cmd.Parameters.AddWithValue("@fn", user.FirstName);
            cmd.Parameters.AddWithValue("@ln", user.LastName);
            cmd.Parameters.AddWithValue("@init", user.Initials ?? "");
            cmd.Parameters.AddWithValue("@login", user.Login);
            cmd.Parameters.AddWithValue("@pass", user.PasswordHash ?? "");

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand("DELETE FROM Users WHERE Id=@id AND Role='Employee'", connection);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        private UserDto Map(SqlDataReader r)
        {
            return new UserDto
            {
                Id = r.GetInt32(0),
                FirstName = r.GetString(1),
                LastName = r.GetString(2),
                Initials = r.IsDBNull(3) ? null : r.GetString(3),
                Login = r.GetString(4),
                Role = r.GetString(5),
                DateOfRegistration = r.GetDateTime(6)
            };
        }
    }
}

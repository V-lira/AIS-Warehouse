using AIS.Warehouse.Data.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class AdminRepository
    {
        private readonly string _connectionString;
        public AdminRepository()
        {
            _connectionString = DbConnectionFactory.ConnectionString;
        }
        public List<AdminContactDto> GetAllAdmins()
        {
            var admins = new List<AdminContactDto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(@"SELECT Id, FirstName, LastName, Initials, Login, Role, DateOfRegistration FROM Users
                                            WHERE Role = 'Admin'
                                            ORDER BY LastName, FirstName", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        admins.Add(new AdminContactDto
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            //login = email
                            Email = reader.GetString(4),
                            Role = reader.GetString(5),
                            RegistrationDate = reader.GetDateTime(6)
                        });
                    }
                }
            }

            return admins;
        }
    }
}
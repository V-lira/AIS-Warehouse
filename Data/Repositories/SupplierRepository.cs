using AIS.Warehouse.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class SupplierRepository
    {
        private readonly string _connectionString =
            DbConnectionFactory.ConnectionString;

        // READ ALL
        public List<SupplierDto> GetAll()
        {
            var list = new List<SupplierDto>();

            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                SELECT Id, FirstName, LastName, Initials, DepartmentId, Login, DateOfRegistration
                FROM Suppliers
                ORDER BY LastName, FirstName", connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        // SEARCH
        public List<SupplierDto> Search(string text)
        {
            var list = new List<SupplierDto>();

            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                SELECT Id, FirstName, LastName, Initials, DepartmentId, Login, DateOfRegistration
                FROM Suppliers
                WHERE FirstName LIKE @t OR LastName LIKE @t OR Login LIKE @t
                ORDER BY LastName, FirstName", connection);

            cmd.Parameters.AddWithValue("@t", $"%{text}%");

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        // CREATE
        public void Add(SupplierDto s)
        {
             var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Suppliers
                (FirstName, LastName, Initials, DepartmentId, Login, PasswordHash)
                VALUES (@fn, @ln, @in, @dep, @login, 'HASH')", connection);

            cmd.Parameters.AddWithValue("@fn", s.FirstName);
            cmd.Parameters.AddWithValue("@ln", s.LastName);
            cmd.Parameters.AddWithValue("@in", (object)s.Initials ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dep", s.DepartmentId);
            cmd.Parameters.AddWithValue("@login", s.Login);

            cmd.ExecuteNonQuery();
        }

        // UPDATE
        public void Update(SupplierDto s)
        {
             var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                UPDATE Suppliers SET
                    FirstName=@fn,
                    LastName=@ln,
                    Initials=@in,
                    DepartmentId=@dep,
                    Login=@login
                WHERE Id=@id", connection);

            cmd.Parameters.AddWithValue("@id", s.Id);
            cmd.Parameters.AddWithValue("@fn", s.FirstName);
            cmd.Parameters.AddWithValue("@ln", s.LastName);
            cmd.Parameters.AddWithValue("@in", (object)s.Initials ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dep", s.DepartmentId);
            cmd.Parameters.AddWithValue("@login", s.Login);

            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Delete(int id)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(
                "DELETE FROM Suppliers WHERE Id=@id", connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private SupplierDto Map(SqlDataReader r)
        {
            return new SupplierDto
            {
                Id = r.GetInt32(0),
                FirstName = r.GetString(1),
                LastName = r.GetString(2),
                Initials = r.IsDBNull(3) ? null : r.GetString(3),
                DepartmentId = r.GetInt32(4),
                Login = r.GetString(5),
                DateOfRegistration = r.GetDateTime(6)
            };
        }
    }
}

using AIS.Warehouse.Data.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class CompanyRepository
    {
        private readonly string _connectionString =
            DbConnectionFactory.ConnectionString;

        // READ ALL
        public List<CompanyDto> GetAll()
        {
            var list = new List<CompanyDto>();

            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(
                "SELECT Id, INN, Name, Address FROM Companies", connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CompanyDto
                {
                    Id = reader.GetInt32(0),
                    INN = reader.GetString(1),
                    Name = reader.GetString(2),
                    Address = reader.GetString(3)
                });
            }

            return list;
        }

        // SEARCH
        public List<CompanyDto> Search(string text)
        {
            var list = new List<CompanyDto>();

            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                SELECT Id, INN, Name, Address
                FROM Companies
                WHERE INN LIKE @t OR Name LIKE @t OR Address LIKE @t",
                connection);

            cmd.Parameters.AddWithValue("@t", $"%{text}%");

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CompanyDto
                {
                    Id = reader.GetInt32(0),
                    INN = reader.GetString(1),
                    Name = reader.GetString(2),
                    Address = reader.GetString(3)
                });
            }

            return list;
        }

        // CREATE
        public void Add(CompanyDto c)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Companies (INN, Name, Address)
                VALUES (@inn, @name, @addr)", connection);

            cmd.Parameters.AddWithValue("@inn", c.INN);
            cmd.Parameters.AddWithValue("@name", c.Name);
            cmd.Parameters.AddWithValue("@addr", c.Address);

            cmd.ExecuteNonQuery();
        }

        // UPDATE
        public void Update(CompanyDto c)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                UPDATE Companies SET
                    INN=@inn,
                    Name=@name,
                    Address=@addr
                WHERE Id=@id", connection);

            cmd.Parameters.AddWithValue("@id", c.Id);
            cmd.Parameters.AddWithValue("@inn", c.INN);
            cmd.Parameters.AddWithValue("@name", c.Name);
            cmd.Parameters.AddWithValue("@addr", c.Address);

            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Delete(int id)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(
                "DELETE FROM Companies WHERE Id=@id", connection);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}

using AIS.Warehouse.Data.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class OperationsLogRepository
    {
        private readonly string _cs;

        public OperationsLogRepository(string cs)
        {
            _cs = cs;
        }

        public List<OperationDto> GetAll()
        {
            var list = new List<OperationDto>();

            var c = new SqlConnection(_cs);
            c.Open();

            var cmd = new SqlCommand(@"
                SELECT Id, ItemId, UserId, Quantity, OperationType, OperationDate
                FROM Operations
                ORDER BY OperationDate DESC", c);

            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new OperationDto
                {
                    Id = r.GetInt32(0),
                    ItemId = r.GetInt32(1),
                    UserId = r.GetInt32(2),
                    Quantity = r.GetInt32(3),
                    OperationType = r.GetString(4),
                    OperationDate = r.GetDateTime(5)
                });
            }

            return list;
        }

        public List<OperationDto> Search(string text)
        {
            var list = new List<OperationDto>();

            var c = new SqlConnection(_cs);
            c.Open();

            //поиск по ID или типу операции
            var cmd = new SqlCommand(@"
                SELECT Id, ItemId, UserId, Quantity, OperationType, OperationDate
                FROM Operations
                WHERE CAST(Id AS VARCHAR) LIKE @text OR
                      OperationType LIKE @text
                ORDER BY OperationDate DESC", c);

            cmd.Parameters.AddWithValue("@text", $"%{text}%");

            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new OperationDto
                {
                    Id = r.GetInt32(0),
                    ItemId = r.GetInt32(1),
                    UserId = r.GetInt32(2),
                    Quantity = r.GetInt32(3),
                    OperationType = r.GetString(4),
                    OperationDate = r.GetDateTime(5)
                });
            }

            return list;
        }

        public void Delete(int id)
        {
            var c = new SqlConnection(_cs);
            c.Open();

            var cmd = new SqlCommand("DELETE FROM Operations WHERE Id = @Id", c);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
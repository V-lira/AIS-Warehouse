using System;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class LogRepository
    {
        private readonly string _connectionString;

        public LogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(int userId, string action, string entity, int? entityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new SqlCommand(@"
                    INSERT INTO Logs (UserId, Action, Entity, EntityId, LogDate)
                    VALUES (@UserId, @Action, @Entity, @EntityId, GETDATE())
                ", connection);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@Entity", entity);
                cmd.Parameters.AddWithValue("@EntityId", entityId ?? (object)DBNull.Value);


                cmd.ExecuteNonQuery();
            }
        }
    }
}
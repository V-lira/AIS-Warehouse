using AIS.Warehouse.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS.Warehouse.Data.Repositories
{
    public class OperationRepository
    {
        private readonly string _connectionString;

        public OperationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<OperationDto> GetJournal()
        {
            var result = new List<OperationDto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var cmd = new SqlCommand(@"SELECT 
                            o.Id,
                            o.ItemId,
                            o.UserId,
                            o.Quantity,
                            o.OperationType,
                            o.OperationDate,
                            i.Name AS ItemName
                        FROM Operations o
                        JOIN Items i ON i.Id = o.ItemId
                        ORDER BY o.OperationDate DESC", connection);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new OperationDto
                            {
                                Id = reader.GetInt32(0),
                                ItemId = reader.GetInt32(1),
                                UserId = reader.GetInt32(2),
                                Quantity = reader.GetInt32(3),
                                OperationType = reader.GetString(4),
                                OperationDate = reader.GetDateTime(5),
                                // ItemName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                            });
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Ошибка базы данных при получении журнала:\n{sqlEx.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении журнала:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        public void AddOperation(int itemId, int userId, string operationType, int quantity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var cmd = new SqlCommand(@"INSERT INTO Operations (ItemId, UserId, OperationType, Quantity, OperationDate)
                                                VALUES (@ItemId, @UserId, @Type, @Quantity, GETDATE())", connection);

                    cmd.Parameters.AddWithValue("@ItemId", itemId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Type", operationType);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Операция успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Ошибка базы данных при добавлении операции:\n{sqlEx.Message}\nКод ошибки: {sqlEx.Number}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении операции:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Дополнительный метод для тестирования подключения
        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
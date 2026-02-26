using AIS.Warehouse.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AIS.Warehouse.Data.Repositories
{
    public class ItemRepository
    {
        private readonly string _connectionString;

        // ДВА конструктора для гибкости
        public ItemRepository() : this(DbConnectionFactory.ConnectionString)
        {
        }

        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString ??
                throw new ArgumentNullException(nameof(connectionString));
        }

        // Метод CREATE (который вы пытаетесь вызвать)
        public void Create(ItemDto item)
        {
            Add(item); // Просто вызываем существующий метод Add
        }

        // READ ALL
        public List<ItemDto> GetAll()
        {
            var items = new List<ItemDto>();

            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT Id, ItemCode, Name, Price, CompanyId
                FROM Items
                ORDER BY Name", connection))
            {
                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(Map(reader));
                    }
                }
            }

            return items;
        }

        // GET BY ID
        public ItemDto GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT Id, ItemCode, Name, Price, CompanyId
                FROM Items
                WHERE Id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Map(reader);
                    }
                }
            }

            return null;
        }

        // SEARCH
        public List<ItemDto> Search(string text)
        {
            var items = new List<ItemDto>();

            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT Id, ItemCode, Name, Price, CompanyId
                FROM Items
                WHERE ItemCode LIKE @t OR Name LIKE @t
                ORDER BY Name", connection))
            {
                cmd.Parameters.AddWithValue("@t", $"%{text}%");
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(Map(reader));
                    }
                }
            }

            return items;
        }

        // CREATE/ADD - С ДВУМЯ ВАРИАНТАМИ
        public void Add(ItemDto item)
        {
            try
            {
                // Вариант 1: С предварительной проверкой и авто-генерацией кода
                if (IsItemCodeExists(item.ItemCode))
                {
                    item.ItemCode = GenerateUniqueItemCode(item.ItemCode);
                }

                using (var connection = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO Items (ItemCode, Name, Price, CompanyId)
                    VALUES (@code, @name, @price, @company);
                    SELECT SCOPE_IDENTITY();", connection))
                {
                    cmd.Parameters.AddWithValue("@code", item.ItemCode);
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@price", item.Price);
                    cmd.Parameters.AddWithValue("@company", item.CompanyId);

                    connection.Open();

                    var newId = cmd.ExecuteScalar();
                    if (newId != null && newId != DBNull.Value)
                    {
                        item.Id = Convert.ToInt32(newId);
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // Если все равно возникла ошибка уникальности
                throw new InvalidOperationException($"Товар с кодом '{item.ItemCode}' уже существует. Пожалуйста, введите другой код.", ex);
            }
        }

        // ДОПОЛНИТЕЛЬНЫЙ МЕТОД: Add с проверкой и без авто-генерации (если нужно спрашивать пользователя)
        public bool TryAdd(ItemDto item, out string errorMessage)
        {
            errorMessage = null;

            if (IsItemCodeExists(item.ItemCode))
            {
                errorMessage = $"Товар с кодом '{item.ItemCode}' уже существует.";
                return false;
            }

            try
            {
                Add(item);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private bool IsItemCodeExists(string itemCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Items WHERE ItemCode = @code", connection))
            {
                cmd.Parameters.AddWithValue("@code", itemCode);
                connection.Open();
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
        }

        private string GenerateUniqueItemCode(string baseCode)
        {
            int counter = 1;
            string testCode = baseCode;

            while (IsItemCodeExists(testCode))
            {
                // Пытаемся найти номер в коде (например, NEW-001 -> 1)
                string cleanCode = baseCode;
                int lastDashIndex = baseCode.LastIndexOf('-');

                if (lastDashIndex > 0 && lastDashIndex < baseCode.Length - 1)
                {
                    string suffix = baseCode.Substring(lastDashIndex + 1);
                    if (int.TryParse(suffix, out int existingCounter))
                    {
                        cleanCode = baseCode.Substring(0, lastDashIndex);
                        counter = existingCounter + 1;
                    }
                }

                testCode = $"{cleanCode}-{counter}";
                counter++;

                // Защита от бесконечного цикла
                if (counter > 1000)
                {
                    // Генерируем GUID если не удается найти уникальный код
                    testCode = $"{cleanCode}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
                    break;
                }
            }

            return testCode;
        }

        // UPDATE с проверкой уникальности
        public void Update(ItemDto item)
        {
            // Проверяем, не занят ли новый код другим товаром
            if (ItemCodeExists(item.ItemCode, item.Id))
            {
                throw new InvalidOperationException($"Товар с кодом '{item.ItemCode}' уже существует. Пожалуйста, введите другой код.");
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE Items SET
                    ItemCode = @code,
                    Name = @name,
                    Price = @price,
                    CompanyId = @company
                WHERE Id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Parameters.AddWithValue("@code", item.ItemCode);
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@price", item.Price);
                cmd.Parameters.AddWithValue("@company", item.CompanyId);

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Товар с ID {item.Id} не найден.");
                }
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM Items WHERE Id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Товар с ID {id} не найден.");
                }
            }
        }

        // CHECK IF ITEM CODE EXISTS (с исключением текущего ID)
        public bool ItemCodeExists(string itemCode, int? excludeId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM Items
                WHERE ItemCode = @code
                    AND (@excludeId IS NULL OR Id != @excludeId)", connection))
            {
                cmd.Parameters.AddWithValue("@code", itemCode);
                cmd.Parameters.AddWithValue("@excludeId", excludeId.HasValue ? (object)excludeId.Value : DBNull.Value);

                connection.Open();
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
        }

        // ПОЛУЧИТЬ ПО КОДУ
        public ItemDto GetByItemCode(string itemCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT Id, ItemCode, Name, Price, CompanyId
                FROM Items
                WHERE ItemCode = @code", connection))
            {
                cmd.Parameters.AddWithValue("@code", itemCode);
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Map(reader);
                    }
                }
            }

            return null;
        }

        private ItemDto Map(SqlDataReader r)
        {
            return new ItemDto
            {
                Id = r.GetInt32(r.GetOrdinal("Id")),
                ItemCode = r.GetString(r.GetOrdinal("ItemCode")),
                Name = r.GetString(r.GetOrdinal("Name")),
                Price = r.GetDecimal(r.GetOrdinal("Price")),
                CompanyId = r.GetInt32(r.GetOrdinal("CompanyId"))
            };
        }

        // Дополнительный метод для проверки количества товаров
        public int GetCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Items", connection))
            {
                connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
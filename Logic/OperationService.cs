using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System;
using System.Collections.Generic;

namespace AIS.Warehouse.Logic
{
    public class OperationService
    {
        private readonly OperationRepository _operationRepo;
        private readonly LogService _logService;

        public OperationService()
        {
            string connectionString = @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
            _operationRepo = new OperationRepository(connectionString);
            _logService = new LogService();
        }

        public void CreateOperation(int itemId, int userId, string operationType, int quantity)
        {
            // Валидация
            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть больше 0");

            string upperType = operationType?.ToUpper();
            if (upperType != "IN" && upperType != "OUT")
                throw new ArgumentException($"Тип операции некорректен: '{operationType}'. Должно быть 'IN' или 'OUT'");

            try
            {
                // Добавление операции
                _operationRepo.AddOperation(itemId, userId, upperType, quantity);

                // Логирование успешного создания
                _logService.Log(
                    userId,
                    $"Создана операция ({upperType}) для товара ID={itemId}, количество: {quantity}",
                    "Operation",
                    null  // или можно получить ID новой операции если нужно
                );
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _logService.Log(
                    userId,
                    $"Ошибка при создании операции: {ex.Message}",
                    "OperationError",
                    null
                );
                throw;
            }
        }

        public List<OperationDto> GetJournal()
        {
            return _operationRepo.GetJournal();
        }
    }
}
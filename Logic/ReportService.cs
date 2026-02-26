using AIS.Warehouse.Data.Repositories;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Windows;

namespace AIS.Warehouse.Logic
{
    public class ReportService
    {
        private readonly OperationRepository _operationRepo;
        private readonly LogService _logService;

        public ReportService()
        {
            _operationRepo = new OperationRepository(
                @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;"
            );
            _logService = new LogService();
        }

        public void CreateOperationsExcel(int userId)
        {
            try
            {
                var operations = _operationRepo.GetJournal();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Журнал операций");
                    //заголовочки
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "ID товара";
                    worksheet.Cell(1, 3).Value = "Количество";
                    worksheet.Cell(1, 4).Value = "Тип операции";
                    worksheet.Cell(1, 5).Value = "Дата операции";
                    //их стиль
                    var headerRow = worksheet.Row(1);
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                    //данные
                    int row = 2;
                    foreach (var op in operations)
                    {
                        worksheet.Cell(row, 1).Value = op.Id;
                        worksheet.Cell(row, 2).Value = op.ItemId;
                        worksheet.Cell(row, 3).Value = op.OperationType;
                        worksheet.Cell(row, 4).Value = op.OperationDate;
                        worksheet.Cell(row, 5).Style.DateFormat.Format = "dd.MM.yyyy HH:mm";
                        row++;
                    }
                    //автоширина для колонок
                    worksheet.Columns().AdjustToContents();
                    //сэйв
                    var fileName = $"Журнал_операций_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePath = Path.Combine(desktopPath, fileName);
                    workbook.SaveAs(filePath);
                    _logService.Log( userId,$"Сформирован Excel-отчет. Файл: {fileName}. Записей: {operations.Count}","Report",null);
                    MessageBox.Show($"Файл сохранен: {filePath}\nЭкспортировано записей: {operations.Count}");
                }
            }
            catch (Exception ex)
            {
                _logService.Log(userId,$"Ошибка при создании Excel-отчета: {ex.Message}","ReportError",null);
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}");
            }
        }
    }
}
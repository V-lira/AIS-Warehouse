using AIS.Warehouse.Data.Repositories;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace AIS.Warehouse.Logic
{
    public class WordReportService
    {
        private readonly OperationRepository _repo;
        private readonly LogService _logService;

        public WordReportService()
        {
            string connectionString = @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
            _repo = new OperationRepository(connectionString);
            _logService = new LogService();
        }
        public void CreateOperationsWord(int userId)
        {
            try
            {
                var operations = _repo.GetJournal();

                var fileName = $"Журнал_операций_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, fileName);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    //загаловок:
                    Paragraph title = new Paragraph();
                    Run titleRun = new Run();
                    Text titleText = new Text("ЖУРНАЛ ОПЕРАЦИЙ");
                    titleRun.Append(titleText);
                    titleRun.RunProperties = new RunProperties(
                        new Bold(),
                        new FontSize() { Val = "32" }
                    );
                    title.Append(titleRun);
                    body.Append(title);
                    body.Append(new Paragraph(new Run(new Text(""))));
                    //адд табличка:
                    Table table = new Table();
                    //стиль таблички:
                    TableProperties tableProperties = new TableProperties(
                        new TableBorders(
                            new TopBorder() { Val = BorderValues.Single, Size = 4 },
                            new BottomBorder() { Val = BorderValues.Single, Size = 4 },
                            new LeftBorder() { Val = BorderValues.Single, Size = 4 },
                            new RightBorder() { Val = BorderValues.Single, Size = 4 },
                            new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 2 },
                            new InsideVerticalBorder() { Val = BorderValues.Single, Size = 2 }
                        )
                    );
                    table.AppendChild(tableProperties);
                    //загаловки таблички
                    TableRow headerRow = new TableRow();
                    string[] headers = { "ID", "Товар", "Количество", "Тип", "Дата" };
                    foreach (var header in headers)
                    {
                        TableCell cell = new TableCell();
                        cell.Append(new Paragraph(new Run(new Text(header))));
                        cell.TableCellProperties = new TableCellProperties(new Shading() { Fill = "D9D9D9" });
                        headerRow.Append(cell);
                    }
                    table.Append(headerRow);
                    //записи до 50 (можно отредачить потом)
                    foreach (var op in operations.Take(50))
                    {
                        TableRow row = new TableRow();
                        //айди
                        row.Append(new TableCell(new Paragraph(new Run(new Text(op.Id.ToString())))));
                        //товары
                        string itemName = $"Товар ID: {op.ItemId}";
                        row.Append(new TableCell(new Paragraph(new Run(new Text(itemName)))));
                        //ип операции
                        string operationType = op.OperationType == "IN" 
                            ? "Приход" 
                            : "Расход";
                        row.Append(new TableCell(new Paragraph(new Run(new Text(operationType)))));
                        //дата
                        row.Append(new TableCell(new Paragraph(new Run(new Text(op.OperationDate.ToString("dd.MM.yyyy HH:mm"))))));
                        table.Append(row);
                    }
                    body.Append(table);
                    Paragraph summary = new Paragraph();
                    Run summaryRun = new Run();
                    Text summaryText = new Text($"Всего операций: {operations.Count}. Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}");
                    summaryRun.Append(summaryText);
                    summary.Append(summaryRun);
                    body.Append(summary);
                }
                _logService.Log(userId,$"Сформирован Word-отчет. Файл: {fileName}. Записей: {operations.Count}","Report",null);
                MessageBox.Show($"Word-файл создан: {filePath}\nЗаписей: {operations.Count}");
            }
            catch (Exception ex)
            {
                _logService.Log(userId, $"Ошибка при создании Word-отчета: {ex.Message}","ReportError",null );

                MessageBox.Show($"Ошибка при создании Word-отчета: {ex.Message}");
            }
        }
    }
}
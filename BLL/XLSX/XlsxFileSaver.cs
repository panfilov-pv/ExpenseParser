using BLL.Abstraction;
using Model.Abstraction;
using OfficeOpenXml;
using System.Drawing;

namespace BLL.XLSX
{
    public class XlsxFileSaver : IFileSaver
    {
        private readonly Color[] _knownColors = new Color[] { Color.LightBlue, Color.LightPink };

        public void Save(string savePath, ICollection<IPreparedRow> preparedRows, IList<string> columns)
        {
            if (string.IsNullOrEmpty(savePath))
            {
                throw new ArgumentException($"'{nameof(savePath)}' cannot be null or empty.", nameof(savePath));
            }

            if (preparedRows is null)
            {
                throw new ArgumentNullException(nameof(preparedRows));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Выгрузка операций по банкам");

            // Подготовка заголовков.
            for (int i = 0; i < columns.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = columns[i];
            }

            int rowsCounter = 2;

            // Запись подготовленных строк
            foreach (IPreparedRow preparedRow in preparedRows)
            {
                worksheet.Cells[rowsCounter, 1].Style.Numberformat.Format = "dd.MM.yyyy HH:mm:ss";
                worksheet.Cells[rowsCounter, 1].Value = preparedRow.OperationDate.Value.ToString("dd.MM.yyyy HH:mm:ss");

                worksheet.Cells[rowsCounter, 2].Value = preparedRow.CategoryName;

                if (!preparedRow.IsKnownCategory)
                {
                    worksheet.Cells[rowsCounter, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rowsCounter, 2].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                }

                worksheet.Cells[rowsCounter, 3].Value = preparedRow.OperationSum;
                worksheet.Cells[rowsCounter, 4].Value = preparedRow.OperationDescription;

                worksheet.Cells[rowsCounter, 5].Value = preparedRow.OperationCardHolder;

                if (preparedRow.IsCardHolderFinded)
                {
                    Color color = _knownColors[preparedRow.ResolvedCardHolderIndex.Value];
                    worksheet.Cells[rowsCounter, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rowsCounter, 5].Style.Fill.BackgroundColor.SetColor(color);
                }

                rowsCounter++;
            }

            // Авторасширение столбцов для удобочитаемости.
            worksheet.Cells.AutoFitColumns();

            // Сохранение файла.
            File.WriteAllBytes(GenerateFileName(savePath), package.GetAsByteArray());
        }

        private string GenerateFileName(string savePath)
        {
            return savePath + "/" + "Выгрузка операций по банкам_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx";
        }
    }
}

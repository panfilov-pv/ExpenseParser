using DAL.Abstraction;
using Model.Abstraction;
using Model.DTO;
using OfficeOpenXml;

namespace DAL
{
    public class SberbankXlsxFileParser : BaseXlsxFileParser
    {
        public override Dictionary<string, ColumnType> ColumnNames { get; } = new()
        {
            { "Дата", ColumnType.OperationDate },
            { "Категория" , ColumnType.CategoryName },
            { "Сумма" , ColumnType.OperationSum },
            { "Описание" , ColumnType.OperationDescription },
            { "Номер счета/карты списания" , ColumnType.OperationCardHolder },
        };
        public override Dictionary<int, string> ColumnNamesByNumbers { get; } = new();

        public override ICollection<IParsedRow> Parse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"По пути {filePath} не найден файл для парсинга!");
            }

            using var package = new ExcelPackage(filePath);

            var worksheet = package.Workbook.Worksheets[0];

            int rowCount = worksheet.Dimension.Rows;
            int columnCount = worksheet.Dimension.Columns;

            if (rowCount == 0 || columnCount == 0)
            {
                return new List<IParsedRow>();
            }

            for (int col = 1; col <= columnCount; col++)
            {
                var cellValue = worksheet.Cells[1, col].Value?.ToString();

                if (cellValue == null || !ColumnNames.ContainsKey(cellValue))
                {
                    continue;
                }

                ColumnNamesByNumbers[col] = cellValue;
            }

            if (ColumnNamesByNumbers.Count == 0)
            {
                return new List<IParsedRow>();
            }

            if (ColumnNamesByNumbers.Count != ColumnNames.Count)
            {
                throw new Exception("Не удалось получить все необходимые для парсинга столбцы!");
            }

            IList<IParsedRow> parsedRows = new List<IParsedRow>(rowCount - 1);

            for (int row = 2; row <= rowCount; row++)
            {
                DateTime? date = null;
                double sum = 0;
                string category = null;
                string description = null;
                string cardHolder = null;

                foreach (var columnNameByNumber in ColumnNamesByNumbers)
                {
                    var excelRange = worksheet.Cells[row, columnNameByNumber.Key];

                    switch (ColumnNames[columnNameByNumber.Value])
                    {
                        case ColumnType.OperationDate:
                            date = GetOperationDate(excelRange); break;
                        case ColumnType.OperationSum: sum = GetOperationSum(excelRange); break;
                        case ColumnType.CategoryName: category = GetCategoryName(excelRange); break;
                        case ColumnType.OperationDescription: description = GetOperationDescription(excelRange); break;
                        case ColumnType.OperationCardHolder: cardHolder = GetOperationCardHolder(excelRange); break;
                        default: throw new Exception("Ошибка: попался неизвестный тип столбца для парсинга!");
                    }
                }

                IParsedRow parsedRow = new ParsedRow(date, category, sum, description, cardHolder);

                if (!parsedRow.IsValid)
                {
                    throw new Exception($"Строка: {row}. Невалидный объект строки!");
                }

                parsedRows.Add(parsedRow);
            }

            return parsedRows;
        }
    }
}
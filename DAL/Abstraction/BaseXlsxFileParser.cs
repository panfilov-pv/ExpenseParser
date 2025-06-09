using Model;
using Model.Abstraction;
using OfficeOpenXml;

namespace DAL.Abstraction
{
    public enum ColumnType
    {
        OperationDate = 0,
        OperationSum = 1,
        CategoryName = 2,
        OperationDescription = 3,
        OperationCardHolder = 4,
        OperationStatus = 5
    }

    public abstract class BaseXlsxFileParser : IFileLoader
    {
        public abstract ICollection<IParsedRow> Parse(string filePath);

        public abstract Dictionary<string, ColumnType> ColumnNames { get; }

        public abstract Dictionary<int, string> ColumnNamesByNumbers { get; }

        public virtual DateTime GetOperationDate(object fieldValue)
        {
            if (fieldValue is DateTime dateTime)
            {
                return dateTime;
            }
            else if (fieldValue is ExcelRange excelRange)
            {
                return excelRange.GetValue<DateTime>();
            }

            throw new InvalidDataException("Не удалось получить дату!");
        }

        public virtual double GetOperationSum(object fieldValue)
        {
            if (fieldValue is double opertaionSum)
            {
                return opertaionSum;
            }
            else if (fieldValue is ExcelRange excelRange)
            {
                return excelRange.GetValue<double>();
            }

            throw new InvalidDataException("Не удалось получить сумму операции!");
        }

        public virtual string GetCategoryName(object fieldValue)
        {
            if (fieldValue is string categoryName)
            {
                return categoryName;
            }
            else if (fieldValue is ExcelRange excelRange)
            {
                return excelRange.GetValue<string>();
            }

            throw new InvalidDataException("Не удалось получить сумму операции!");
        }

        public virtual string GetOperationDescription(object fieldValue)
        {
            if (fieldValue is string operationDescription)
            {
                return operationDescription;
            }
            else if (fieldValue is ExcelRange excelRange)
            {
                return excelRange.GetValue<string>();
            }

            throw new InvalidDataException("Не удалось получить сумму операции!");
        }

        public virtual string GetOperationCardHolder(object fieldValue)
        {
            if (fieldValue is string operationCardHolder)
            {
                return operationCardHolder;
            }
            else if (fieldValue is ExcelRange excelRange)
            {
                return excelRange.GetValue<string>();
            }

            throw new InvalidDataException("Не удалось получить сумму операции!");
        }

        public virtual OperationStatus GetOperationStatus(object fieldValue)
        {
            if (fieldValue is string operationDescription)
            {
                return operationDescription switch
                {
                    "OK" => OperationStatus.OK,
                    "FAILED" => OperationStatus.FAIL,
                    _ => throw new Exception("Не удалось распарсить статус операции!"),
                };
            }
            else if (fieldValue is ExcelRange excelRange)
            {
                return excelRange.GetValue<string>() switch
                {
                    "OK" => OperationStatus.OK,
                    "FAILED" => OperationStatus.FAIL,
                    _ => throw new Exception("Не удалось распарсить статус операции!"),
                };
            }

            throw new InvalidDataException("Не удалось получить статус операции!");
        }
    }
}

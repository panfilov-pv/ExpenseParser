using GemBox.Spreadsheet;

namespace Shared.Converters
{
    public static class ExcelConverter
    {
        public static void ConvertToXlsx(string inputFilePath, string outputFilePath)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile? excelFile = ExcelFile.Load(inputFilePath);

            excelFile.Save(outputFilePath);
        }
    }
}

using BLL.Abstraction;
using DAL;
using DAL.Abstraction;
using Model.Abstraction;
using Shared.Configuration;

namespace BLL
{
    public class Worker : IWorker
    {
        private readonly IConfig _config;
        private readonly IFileLoader _sberbankXlsxParser;
        private readonly IFileLoader _tinkoffXlsxParser;
        private readonly IDataPreparer _dataPreparer;
        private readonly IFileSaver _fileSaver;

        public Worker(IConfig config,
            SberbankXlsxFileParser sberbankXlsxParser,
            TinkoffXlsxFileParser tinkoffXlsxParser,
            IDataPreparer dataPreparer,
            IFileSaver fileSaver)
        {
            _config = config;
            _sberbankXlsxParser = sberbankXlsxParser;
            _tinkoffXlsxParser = tinkoffXlsxParser;
            _dataPreparer = dataPreparer;
            _fileSaver = fileSaver;
        }

        public void Work()
        {
            List<IParsedRow> parsedRows = ParseRows();

            ICollection<IPreparedRow> preparedRows = _dataPreparer.PrepareData(parsedRows);

            _fileSaver.Save(_config.ResultFilePath, preparedRows,
                new List<string> { "Дата", "Категория", "Сумма", "Описание", "Номер счета/карты списания" }); // TODO - вынести нужные столбцы в конфиг.
        }

        private List<IParsedRow> ParseRows()
        {
            List<IParsedRow> parsedRows = new();

            string[] sberbankFiles = Directory.GetFiles(_config.SberbankFolderPath);

            if (sberbankFiles.Length > 0)
            {
                foreach (string sberbankFilePath in sberbankFiles)
                {
                    parsedRows.AddRange(_sberbankXlsxParser.Parse(sberbankFilePath));
                }
            }

            string[] tinkoffFiles = Directory.GetFiles(_config.TinkoffFolderPath);

            if (tinkoffFiles.Length > 0)
            {
                foreach (string tinkoffFilePath in tinkoffFiles)
                {
                    parsedRows.AddRange(_tinkoffXlsxParser.Parse(tinkoffFilePath));
                }
            }

            return parsedRows;
        }
    }
}

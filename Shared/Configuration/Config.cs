using Microsoft.Extensions.Configuration;
using Shared.Exceptions;

namespace Shared.Configuration
{
    public class Config : IConfig
    {
        public string SberbankFolderPath { get; private set; }

        public string TinkoffFolderPath { get; private set; }

        public string ResultFilePath { get; private set; }

        public IReadOnlyDictionary<string, HashSet<string>> ResolvesForCategories { get; private set; }

        public string UnknownCategoryName { get; private set; }

        public IReadOnlyDictionary<string, HashSet<string>> KnownCardNumbers { get; private set; }
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;

            Initialize();
        }

        private void Initialize()
        {
            try
            {
                SberbankFolderPath = _configuration.GetSection("AppSettings:sberbank_folder_path")?.Value
                    ?? throw new ConfigInitializeException($"Отсутствует параметр {nameof(SberbankFolderPath)}.");

                TinkoffFolderPath = _configuration.GetSection("AppSettings:tinkoff_folder_path")?.Value
                    ?? throw new ConfigInitializeException($"Отсутствует параметр {nameof(TinkoffFolderPath)}.");

                ResultFilePath = _configuration.GetSection("AppSettings:result_file_path")?.Value
                    ?? throw new ConfigInitializeException($"Отсутствует параметр {nameof(ResultFilePath)}.");

                ResolvesForCategories = GetResolvesForCategories();

                UnknownCategoryName = _configuration.GetSection("AppSettings:unknown_category_name")?.Value
                    ?? throw new ConfigInitializeException($"Отсутствует параметр {nameof(UnknownCategoryName)}.");

                UnknownCategoryName = _configuration.GetSection("AppSettings:unknown_category_name")?.Value
                    ?? throw new ConfigInitializeException($"Отсутствует параметр {nameof(UnknownCategoryName)}.");

                GetKnownCardNumbers();
            }
            catch (ConfigInitializeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConfigInitializeException("Возникла ошибка при попытке " +
                    "проинициализировать конифгурационные данные приложения!", ex);
            }
        }

        private IReadOnlyDictionary<string, HashSet<string>> GetResolvesForCategories()
        {
            var section = _configuration.GetSection("AppSettings:categories") ?? throw new ConfigInitializeException($"Отсутствует параметр {nameof(ResolvesForCategories)}.");

            var container = new Dictionary<string, HashSet<string>>();

            foreach (var categorySection in section.GetChildren())
            {
                container[categorySection.Key] = categorySection.GetChildren().Select(x => x.Value).ToHashSet();
            }

            return container;
        }

        private void GetKnownCardNumbers()
        {
            var container = new Dictionary<string, HashSet<string>>();

            var section = _configuration.GetSection("AppSettings:known_card_numbers");

            if (section == null)
            {
                KnownCardNumbers = container;
                return;
            }

            foreach (var categorySection in section.GetChildren())
            {
                container[categorySection.Key] = categorySection.GetChildren().Select(x => x.Value).ToHashSet();
            }

            KnownCardNumbers = container;
        }
    }
}

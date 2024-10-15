namespace Shared.Configuration
{
    public interface IConfig
    {
        string SberbankFolderPath { get; }

        string TinkoffFolderPath { get; }

        string ResultFilePath { get; }

        IReadOnlyDictionary<string, HashSet<string>> ResolvesForCategories { get; }

        string UnknownCategoryName { get; }

        IReadOnlyDictionary<string, HashSet<string>> KnownCardNumbers { get; }
    }
}

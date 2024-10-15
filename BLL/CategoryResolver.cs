using BLL.Abstraction;
using Shared.Configuration;

namespace BLL
{
    public class CategoryResolver : ICategoryResolver
    {
        private readonly IConfig _config;

        private CategoryResolver() { }

        public CategoryResolver(IConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.ResolvesForCategories is null)
            {
                throw new ArgumentNullException(nameof(config.ResolvesForCategories));
            }

            if (string.IsNullOrEmpty(config.UnknownCategoryName))
            {
                throw new ArgumentNullException(nameof(config.UnknownCategoryName));
            }

            _config = config;
        }
        public bool ResolveCategory(string categoryNameToResolve, out string category)
        {
            category = _config.ResolvesForCategories.FirstOrDefault(r => r.Value.Contains(categoryNameToResolve)).Key;

            if (string.IsNullOrEmpty(category))
            {
                category = _config.UnknownCategoryName + $" ({categoryNameToResolve})";
                return false;
            }

            return true;
        }
    }
}

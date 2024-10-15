namespace BLL.Abstraction
{
    public interface ICategoryResolver
    {
        bool ResolveCategory(string categoryNameToResolve, out string category);
    }
}

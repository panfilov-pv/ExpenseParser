using Model.Abstraction;

namespace DAL.Abstraction
{
    public interface IFileLoader
    {
        public abstract ICollection<IParsedRow> Parse(string filePath);
    }
}

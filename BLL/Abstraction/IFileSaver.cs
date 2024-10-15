using Model.Abstraction;

namespace BLL.Abstraction
{
    public interface IFileSaver
    {
        public void Save(string savePath, ICollection<IPreparedRow> preparedRows, IList<string> columns);
    }
}

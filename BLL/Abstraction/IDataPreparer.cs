using Model.Abstraction;

namespace BLL.Abstraction
{
    public interface IDataPreparer
    {
        public ICollection<IPreparedRow> PrepareData(ICollection<IParsedRow> parsedRows);
    }
}

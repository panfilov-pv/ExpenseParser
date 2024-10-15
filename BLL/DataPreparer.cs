using BLL.Abstraction;
using Model;
using Model.Abstraction;

namespace BLL
{
    public class DataPreparer : IDataPreparer
    {
        private readonly bool _throwIfRowEmpty = true;
        private readonly bool _throwIfRowNotValid = true;
        private readonly bool _needSortByDate = true;

        private readonly ICategoryResolver _categoryResolver;
        private readonly ICardNumberResolver _cardNumberResolver;

        public DataPreparer(ICategoryResolver categoryResolver, ICardNumberResolver cardNumberResolver, bool throwIfRowEmpty = true, bool throwIfRowNotValid = true, bool needSortByDate = true)
        {
            _categoryResolver = categoryResolver ?? throw new ArgumentNullException(nameof(categoryResolver));
            _cardNumberResolver = cardNumberResolver ?? throw new ArgumentNullException(nameof(cardNumberResolver));
            _throwIfRowEmpty = throwIfRowEmpty;
            _throwIfRowNotValid = throwIfRowNotValid;
            _needSortByDate = needSortByDate;
        }

        public ICollection<IPreparedRow> PrepareData(ICollection<IParsedRow> parsedRows)
        {
            ICollection<IPreparedRow> preparedRows = new List<IPreparedRow>();

            if (parsedRows == null || parsedRows.Count == 0)
            {
                return preparedRows;
            }

            if (_needSortByDate)
            {
                parsedRows = parsedRows.OrderBy(x => x.OperationDate).ToList();
            }

            foreach (IParsedRow parsedRow in parsedRows)
            {
                if (parsedRow == null && _throwIfRowEmpty)
                {
                    throw new ArgumentNullException(nameof(parsedRow));
                }

                if (!parsedRow.IsValid && _throwIfRowNotValid)
                {
                    throw new ArgumentException($"Invalid parsed row data! Date: {parsedRow.OperationDate}.");
                }

                bool isResolved = _categoryResolver.ResolveCategory(parsedRow.CategoryName, out string resolvedCategory);

                if (string.IsNullOrEmpty(resolvedCategory))
                {
                    throw new Exception($"Can't resolve category with name {resolvedCategory}!");
                }

                IPreparedRow preparedRow = new PreparedRow(
                    parsedRow.OperationDate,
                    resolvedCategory,
                    parsedRow.OperationSum,
                    parsedRow.OperationDescription,
                    parsedRow.OperationCardHolder)
                {
                    IsKnownCategory = isResolved,
                    ResolvedCardHolderIndex = _cardNumberResolver.ResolveCardNumber(parsedRow.OperationCardHolder)
                };

                preparedRows.Add(preparedRow);
            }

            return preparedRows;
        }
    }
}

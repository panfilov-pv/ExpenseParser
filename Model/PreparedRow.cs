using Model.Abstraction;

namespace Model
{
    public class PreparedRow : IPreparedRow
    {
        public DateTime? OperationDate { get; }

        public string? CategoryName { get; }

        public double OperationSum { get; }

        public string? OperationDescription { get; }

        public string? OperationCardHolder { get; }

        public bool IsKnownCategory { get; set; }

        public int? ResolvedCardHolderIndex { get; set; } = null;

        public bool IsCardHolderFinded => ResolvedCardHolderIndex.HasValue;

        private PreparedRow() { }

        public PreparedRow(DateTime? operationDate, string? category, double operationSum, string? description, string? cardNumber)
        {
            OperationDate = operationDate;
            CategoryName = category;
            OperationSum = operationSum;
            OperationDescription = description;
            OperationCardHolder = cardNumber;
        }
    }
}

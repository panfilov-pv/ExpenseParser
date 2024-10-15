using Model.Abstraction;

namespace Model.DTO
{
    public record ParsedRow : IParsedRow
    {

        public bool IsValid => OperationSum >= 0 && !string.IsNullOrEmpty(CategoryName);

        public DateTime? OperationDate { get; }

        public string? CategoryName { get; }

        public double OperationSum { get; }

        public string? OperationDescription { get; }

        public string? OperationCardHolder { get; }

        public ParsedRow(DateTime? operationDate, string? category, double operationSum, string? description, string? cardNumber)
        {
            OperationDate = operationDate;
            CategoryName = category;
            OperationSum = operationSum;
            OperationDescription = description;
            OperationCardHolder = cardNumber;
        }
    }
}

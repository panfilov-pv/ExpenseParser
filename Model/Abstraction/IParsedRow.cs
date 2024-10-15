namespace Model.Abstraction
{
    public interface IParsedRow
    {
        DateTime? OperationDate { get; }

        string? CategoryName { get; }

        double OperationSum { get; }

        string? OperationDescription { get; }

        string? OperationCardHolder { get; }

        bool IsValid { get; }
    }
}

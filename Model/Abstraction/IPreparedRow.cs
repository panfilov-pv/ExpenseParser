namespace Model.Abstraction
{
    public interface IPreparedRow
    {
        DateTime? OperationDate { get; }

        string? CategoryName { get; }

        double OperationSum { get; }

        string? OperationDescription { get; }

        string? OperationCardHolder { get; }

        bool IsKnownCategory { get; }

        int? ResolvedCardHolderIndex { get; }

        bool IsCardHolderFinded { get; }
    }
}

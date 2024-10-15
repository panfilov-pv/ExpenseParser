namespace BLL.Abstraction
{
    public interface ICardNumberResolver
    {
        int? ResolveCardNumber(string cardNumberToResolve);
    }
}

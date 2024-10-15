namespace Shared.Exceptions
{
    public class ConfigInitializeException : Exception
    {
        public ConfigInitializeException(string? message) : base(message)
        {
        }

        public ConfigInitializeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

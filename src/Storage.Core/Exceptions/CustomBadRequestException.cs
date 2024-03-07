namespace Storage.Core.Exceptions
{
    public abstract class CustomBadRequestException : Exception
    {
        protected CustomBadRequestException(string message) : base(message)
        {
        }
    }
}

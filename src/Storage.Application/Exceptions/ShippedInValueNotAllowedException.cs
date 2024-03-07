using Storage.Core.Exceptions;

namespace Storage.Application.Exceptions
{
    public class ShippedInValueNotAllowedException : CustomBadRequestException
    {
        public ShippedInValueNotAllowedException() : base("Value for parameter ShhipedIn not allowed. Allowed values [24, 48, 72].")
        {
        }
    }
}

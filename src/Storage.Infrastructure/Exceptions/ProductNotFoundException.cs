using Storage.Core.Exceptions;

namespace Storage.Infrastructure.Exceptions
{
    public sealed class ProductNotFoundException : CustomNotFoundException
    {
        public string Sku { get; }

        public ProductNotFoundException(string sku) : base($"Product with '{sku}' was not found.")
        {
            Sku = sku;
        }
    }
}

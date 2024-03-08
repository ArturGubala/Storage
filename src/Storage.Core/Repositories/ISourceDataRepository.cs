using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface ISourceDataRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(int shippedIn, string productNameNotLike);
        Task<IEnumerable<Inventory>> GetInventoryAsync(int shippedIn);
        Task<IEnumerable<Price>> GetPricesAsync(IEnumerable<Product> products);
    }
}

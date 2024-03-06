using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface ISourceDataRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(int shippedIn, string productNameLike);
        Task<IEnumerable<Inventory>> GetInventoryAsync(IEnumerable<Product> products);
        Task<IEnumerable<Price>> GetPricesAsync(IEnumerable<Product> products);
    }
}

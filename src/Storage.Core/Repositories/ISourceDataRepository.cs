using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface ISourceDataRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Inventory>> GetInventoryAsync();
        Task<IEnumerable<Price>> GetPricesAsync();
    }
}

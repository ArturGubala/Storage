using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetBySkuAsync(string sku);
        Task AddManyAsync(IEnumerable<Product> products);
    }
}

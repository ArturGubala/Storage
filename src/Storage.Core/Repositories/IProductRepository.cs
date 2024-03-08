using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface IProductRepository
    {
        Task AddManyAsync(IEnumerable<Product> products);
    }
}

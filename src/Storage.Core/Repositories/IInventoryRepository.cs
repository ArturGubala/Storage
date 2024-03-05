using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface IInventoryRepository
    {
        Task AddManyAsync(IEnumerable<Inventory> inventory);
    }
}

using Storage.Core.Entities;

namespace Storage.Core.Repositories
{
    public interface IPriceRepository
    {
        Task AddManyAsync(IEnumerable<Price> prices);
    }
}

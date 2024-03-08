using Storage.Core.Entities;
using Storage.Core.Repositories;
using Z.Dapper.Plus;

namespace Storage.Infrastructure.DAL.Repositories
{
    internal class PriceRepository : IPriceRepository
    {
        private readonly StorageDbContext _dbContext;

        public PriceRepository(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddManyAsync(IEnumerable<Price> prices)
        {
            DapperPlusManager
                .Entity<Price>()
                .Table("PRICE")
                .Map(price => price.ProductId, "PRODUCT_ID")
                .Map(product => product.NetPriceForUnitOfSale, "NET_PRICE_FOR_UNIT_OF_SALE");

            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                    await connection.BulkActionAsync(x => x.BulkInsert(prices));
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
    }
}

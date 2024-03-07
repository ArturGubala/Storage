using Dapper;
using Storage.Core.Entities;
using Storage.Core.Repositories;

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
            var query = """"
                    INSERT INTO PRICE (PRODUCT_ID, NET_PRICE_FOR_UNIT_OF_SALE)
                    VALUES (@ProductId, @NetPriceForUnitOfSale);
                """";

            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                    await connection.ExecuteAsync(query, prices.ToArray());
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }

        }
    }
}

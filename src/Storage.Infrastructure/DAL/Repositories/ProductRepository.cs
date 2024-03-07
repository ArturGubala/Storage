using Dapper;
using Storage.Core.Entities;
using Storage.Core.Repositories;

namespace Storage.Infrastructure.DAL.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly StorageDbContext _dbContext;

        public ProductRepository(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddManyAsync(IEnumerable<Product> products)
        {
            var query = """"
                    INSERT INTO PRODUCT (ID, SKU, NAME, EAN, PRODUCER_NAME, CATEGORY, IS_WIRE, AVAILABLE, IS_VENDOR, DEFAULT_IMAGE, SHIPPING_TIME)
                    VALUES (@Id, @Sku, @Name, @Ean, @ProducerName, @Category, @IsWire, @Available, @IsVendor, @DefaultImage, @ShippingTime);
                """";

            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                    await connection.ExecuteAsync(query, products.ToArray());
                } 
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }

        }

        public Task<Product> GetBySkuAsync(string sku)
        {
            throw new NotImplementedException();
        }
    }
}

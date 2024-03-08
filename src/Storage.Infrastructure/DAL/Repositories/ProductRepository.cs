using Storage.Core.Entities;
using Storage.Core.Repositories;
using Z.Dapper.Plus;

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
            DapperPlusManager
                .Entity<Product>()
                .Table("PRODUCT")
                .Map(product => product.Id, "ID")
                .Map(product => product.Sku, "SKU")
                .Map(product => product.Name, "NAME")
                .Map(product => product.Ean, "EAN")
                .Map(product => product.ProducerName, "PRODUCER_NAME")
                .Map(product => product.Category, "CATEGORY")
                .Map(product => product.IsWire, "IS_WIRE")
                .Map(product => product.ShippingTime, "SHIPPING_TIME")
                .Map(product => product.Available, "AVAILABLE")
                .Map(product => product.IsVendor, "IS_VENDOR")
                .Map(product => product.DefaultImage, "DEFAULT_IMAGE");

            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                    await connection.BulkActionAsync(x => x.BulkInsert(products.ToArray()));
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

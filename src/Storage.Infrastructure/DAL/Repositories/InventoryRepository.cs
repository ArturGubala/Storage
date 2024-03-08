using Storage.Core.Entities;
using Storage.Core.Repositories;
using Z.Dapper.Plus;

namespace Storage.Infrastructure.DAL.Repositories
{
    internal class InventoryRepository : IInventoryRepository
    {
        private readonly StorageDbContext _dbContext;

        public InventoryRepository(StorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddManyAsync(IEnumerable<Inventory> inventory)
        {
            DapperPlusManager
                .Entity<Inventory>()
                .Table("INVENTORY")
                .Map(inventory => inventory.ProductId, "PRODUCT_ID")
                .Map(inventory => inventory.StockQty, "STOCK_QTY")
                .Map(inventory => inventory.SaleUnit, "SALE_UNIT")
                .Map(inventory => inventory.ShippingCost, "SHIPPING_COST");

            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                    await connection.BulkActionAsync(x => x.BulkInsert(inventory.ToArray()));
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }
        }
    }
}

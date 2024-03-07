using Dapper;
using Storage.Core.Entities;
using Storage.Core.Repositories;

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
            var query = """"
                    insert into INVENTORY (PRODUCT_ID, STOCK_QTY, SALE_UNIT, SHIPPING_COST)
                    values (@ProductId, @StockQty, @SaleUnit, @ShippingCost);    
                """";


            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                   await connection.ExecuteAsync(query, inventory.ToArray());
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }
        }
    }
}

using Dapper;
using Storage.Application.Abstractions;
using Storage.Application.DTO;
using Storage.Infrastructure.DAL;

namespace Storage.Application.Queries.Handlers
{
    internal sealed class GetProductHendler : IQueryHandler<GetProduct, ProductDto>
    {
        private readonly StorageDbContext _dbContext;

        public GetProductHendler(StorageDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<ProductDto> HandleAsync(GetProduct query)
        {
            ProductDto? productDto = default;

            var productInfoQuery = """
                    SELECT PR.ID, PR.NAME, PR.EAN, PR.PRODUCER_NAME, 
                           PR.CATEGORY, PR.DEFAULT_IMAGE, I.STOCK_QTY, 
                           I.SALE_UNIT, PC.NET_PRICE_FOR_UNIT_OF_SALE, 
                           I.SHIPPING_COST
                    FROM PRODUCT PR
                    JOIN INVENTORY I ON I.PRODUCT_ID = PR.ID
                    JOIN PRICE PC ON PC.PRODUCT_ID = PR.ID
                    WHERE PR.SKU = @Sku
                """;

            using (var connection = _dbContext.CreateConnection())
            {
                try
                {
                    var productEntity =
                        await connection
                            .QueryFirstOrDefaultAsync<(int? Id, string Name, string Ean, string ProducerName, string Category, 
                                string DefaultImage, double StockQty, string SaleUnit, decimal NetPriceForUnitOfSale, decimal ShippingCost)>
                            (productInfoQuery, new { query.Sku });

                    // TODO: Is it best way to check if product was found? How to got null for entire entity?
                    if (productEntity.Id == null)
                    {
                        return productDto;
                    }

                    productDto = new ProductDto()
                    {
                        Name = productEntity.Name,
                        Ean = productEntity.Ean,
                        ProducerName = productEntity.ProducerName,
                        Category = productEntity.Category,
                        DefaultImage = productEntity.DefaultImage,
                        StockQty = productEntity.StockQty,
                        SaleUnit = productEntity.SaleUnit,
                        NetPriceForUnitOfSale = productEntity.NetPriceForUnitOfSale,
                        ShippingCost = productEntity.ShippingCost,
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }

            return productDto;
        }
    }
}

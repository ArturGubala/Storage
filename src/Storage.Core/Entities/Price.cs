namespace Storage.Core.Entities
{
    public class Price
    {
        public int ProductId { get; private set; } = 0;
        public string Sku { get; private set; }
        public decimal NetPriceForUnitOfSale { get; private set; }

        public Price(int productId, string sku, decimal netPriceForUnitOfSale)
        {
            ProductId = productId;
            Sku = sku;
            NetPriceForUnitOfSale = netPriceForUnitOfSale;
        }
    }
}

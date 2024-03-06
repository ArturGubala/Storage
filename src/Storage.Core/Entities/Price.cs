namespace Storage.Core.Entities
{
    public class Price
    {
        public int ProductId { get; private set; }
        public decimal NetPriceForUnitOfSale { get; private set; }

        public Price(int productId, decimal netPriceForUnitOfSale)
        {
            ProductId = productId;
            NetPriceForUnitOfSale = netPriceForUnitOfSale;
        }
    }
}

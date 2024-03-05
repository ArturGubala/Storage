namespace Storage.Core.Entities
{
    public class Price
    {
        public int ProductId { get; private set; }
        public decimal NetPriceForUnitOfSale { get; private set; }

        private Price(int productId, decimal netPriceForUnitOfSale)
        {
            ProductId = productId;
            NetPriceForUnitOfSale = netPriceForUnitOfSale;
        }

        public Price Create(int productId, decimal netPriceForUnitOfSale)
            => new Price(productId, netPriceForUnitOfSale);
    }
}

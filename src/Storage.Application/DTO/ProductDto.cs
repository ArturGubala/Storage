namespace Storage.Application.DTO
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Ean { get; set; }
        public string ProducerName { get; set; }
        public string Category { get; set; }
        public string DefaultImage { get; set; }
        public double? StockQty { get; set; }
        public string SaleUnit { get; set; }
        public decimal? NetPriceForUnitOfSale { get; set; }
        public decimal? ShippingCost { get; set; }
    }
}
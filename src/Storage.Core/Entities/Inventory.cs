namespace Storage.Core.Entities
{
    public class Inventory
    {
        public int ProductId { get; private set; }
        public int StockQty { get; private set; }
        public string SaleUnit { get; private set; }
        public decimal ShippingCost { get; private set; }

        private Inventory(int productId, int stockQty, string saleUnit, decimal shippingCost) 
        { 
            ProductId = productId;
            StockQty = stockQty;
            SaleUnit = saleUnit;
            ShippingCost = shippingCost;
        }

        public Inventory Create(int productId, int stockQty, string saleUnit, decimal shippingCost)
            => new(productId, stockQty, saleUnit, shippingCost);
    }
}

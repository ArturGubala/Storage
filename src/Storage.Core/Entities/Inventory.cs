namespace Storage.Core.Entities
{
    public class Inventory
    {
        public int ProductId { get; private set; }
        public double StockQty { get; private set; }
        public string SaleUnit { get; private set; }
        public decimal ShippingCost { get; private set; }

        public Inventory(int productId, double stockQty, string saleUnit, decimal shippingCost) 
        { 
            ProductId = productId;
            StockQty = stockQty;
            SaleUnit = saleUnit;
            ShippingCost = shippingCost;
        }
    }
}

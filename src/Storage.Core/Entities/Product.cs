namespace Storage.Core.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public string Ean { get; private set; }
        public string ProducerName { get; private set; }
        public string Category { get; private set; }
        public int IsWire { get; private set; }
        public int Available { get; private set; }
        public int IsVendor { get; private set; }
        public string DefaultImage { get; private set; }

        public Product(int id, string sku, string name, string ean, string producerName, string category,
                        int  isWire, int available, int isVendor, string defaultImage)
        {
            Id = id;
            Sku = sku;
            Name = name;
            Ean = ean;
            ProducerName = producerName;
            Category = category;
            IsWire = isWire;
            Available = available;
            IsVendor = isVendor;
            DefaultImage = defaultImage;
        }
    }
}

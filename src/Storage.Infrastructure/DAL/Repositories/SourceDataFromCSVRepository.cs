using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using Storage.Core.Entities;
using Storage.Core.Repositories;
using Storage.Infrastructure.DAL.Models;
using System.Globalization;

namespace Storage.Infrastructure.DAL.Repositories
{
    internal sealed class SourceDataFromCSVRepository : ISourceDataRepository
    {
        private readonly SourceCsvDataOptions _options;

        public SourceDataFromCSVRepository(IOptions<SourceCsvDataOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IEnumerable<Inventory>> GetInventoryAsync()
        {
            List<Inventory> inventories = new List<Inventory>();
            Stream inventoryData = await RequestForDataAsync(_options.InventoryFileOptions.FileName);
            CsvConfiguration configuration = GetCsvConfiguration(CultureInfo.InvariantCulture, _options.InventoryFileOptions.Delimiter);

            using (StreamReader reader = new StreamReader(inventoryData))
            using (CsvReader csv = new CsvReader(reader, configuration))
            {
                if (_options.InventoryFileOptions.HasHeader)
                {
                    csv.Read();
                }
                while (csv.Read() && csv.TryGetField(0, out int _))
                {
                    csv.TryGetField(7, out decimal shippingCost);
                    Inventory inventory = new Inventory(
                        productId: csv.GetField<int>(0),
                        stockQty: csv.GetField<double>(3),
                        saleUnit: csv.GetField<string>(2),
                        shippingCost: shippingCost
                    );
                    inventories.Add(inventory);
                }
            }

            return inventories;
        }

        public async Task<IEnumerable<Price>> GetPricesAsync()
        {
            List<Price> prices = new List<Price>();
            Stream pricesData = await RequestForDataAsync(_options.PricesFileOptions.FileName);
            CsvConfiguration configuration = GetCsvConfiguration(CultureInfo.InvariantCulture, _options.PricesFileOptions.Delimiter);

            using (StreamReader reader = new StreamReader(pricesData))
            using (CsvReader csv = new CsvReader(reader, configuration))
            {
                if (_options.PricesFileOptions.HasHeader)
                {
                    csv.Read();
                }
                while (csv.Read() && csv.TryGetField(0, out string _))
                {
                    decimal.TryParse(csv.GetField<string>(5).Trim().Replace(',', '.'), CultureInfo.InvariantCulture, out decimal netPriceForUnitOfSale);
                    Price price = new Price
                    (
                        productId: 0,
                        sku: csv.GetField<string>(1),
                        netPriceForUnitOfSale: netPriceForUnitOfSale
                    );
                    prices.Add(price);
                }
            }

            return prices;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            List<Product> products = new List<Product>();
            Stream productData = await RequestForDataAsync(_options.ProductFileOptions.FileName);
            CsvConfiguration configuration = GetCsvConfiguration(CultureInfo.InvariantCulture, _options.ProductFileOptions.Delimiter);

            using (StreamReader reader = new StreamReader(productData))
            using (CsvReader csv = new CsvReader(reader, configuration))
            {
                if (_options.ProductFileOptions.HasHeader)
                {
                    csv.Read();
                }
                while (csv.Read() && csv.TryGetField(0, out int _))
                {
                    Product product = new Product(
                        id: csv.GetField<int>(0),
                        sku: csv.GetField<string>(0),
                        name: csv.GetField<string>(2),
                        ean: csv.GetField<string>(4),
                        producerName: csv.GetField<string>(6),
                        category: csv.GetField<string>(7),
                        isWire: csv.GetField<int>(8),
                        available: csv.GetField<int>(11),
                        isVendor: csv.GetField<int>(16),
                        defaultImage: csv.GetField<string>(18)
                    );
                    products.Add(product);
                }
            }

            return products;
        }

        private async Task<Stream> RequestForDataAsync(string fileName)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var fullUrl = $"{_options.BaseUrl}/{fileName}";
                    HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        // TODO: rzucic odpowiedni wyjatek
                        return null;
                    }
                    
                     return await response.Content.ReadAsStreamAsync();
                }
                catch (Exception ex)
                {
                    // TODO: rzucic odpowiedni wyjatek
                    return null;
                }
            }
        }

        private CsvConfiguration GetCsvConfiguration(CultureInfo culture, string delimiter)
        {
            return new CsvConfiguration(culture)
            {
                Delimiter = delimiter,
            };
        }
    }
}

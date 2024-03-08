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

        public async Task<IEnumerable<Inventory>> GetInventoryAsync(int shippedIn)
        {
            List<Inventory> inventories = new List<Inventory>();
            Stream inventoryData = await RequestForDataAndSaveToFileAsync(_options.InventoryFileOptions.FileName);
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
                    csv.TryGetField(0, out int productId);
                    csv.TryGetField(6, out string shipping);
                    if (!shipping.ToLower().Contains($"{shippedIn}h"))
                    {
                        continue;
                    }
                    csv.TryGetField(7, out decimal shippingCost);
                    Inventory inventory = new Inventory(
                        productId: productId,
                        stockQty: csv.GetField<double>(3),
                        saleUnit: csv.GetField<string>(2),
                        shippingCost: shippingCost
                    );
                    inventories.Add(inventory);
                }
            }

            return inventories;
        }

        public async Task<IEnumerable<Price>> GetPricesAsync(IEnumerable<Product> products)
        {
            List<Price> prices = new List<Price>();
            Stream pricesData = await RequestForDataAndSaveToFileAsync(_options.PricesFileOptions.FileName);
            CsvConfiguration configuration = GetCsvConfiguration(CultureInfo.InvariantCulture, _options.PricesFileOptions.Delimiter);

            using (StreamReader reader = new StreamReader(pricesData))
            using (CsvReader csv = new CsvReader(reader, configuration))
            {
                products = products.ToList();
                if (_options.PricesFileOptions.HasHeader)
                {
                    csv.Read();
                }
                while (csv.Read() && csv.TryGetField(0, out string _))
                {
                    csv.TryGetField(1, out string sku);
                    decimal.TryParse(csv.GetField<string>(5).Trim().Replace(',', '.'), CultureInfo.InvariantCulture, out decimal netPriceForUnitOfSale);
                    Price price = new Price
                    (
                        productId: products.FirstOrDefault(p => p.Sku == sku) == null ? 0 : products.First(p => p.Sku == sku).Id,
                        netPriceForUnitOfSale: netPriceForUnitOfSale
                    );
                    prices.Add(price);
                }
            }

            return prices;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int shippedIn, string productNameNotLike)
        {
            List<Product> products = new List<Product>();
            Stream productData = await RequestForDataAndSaveToFileAsync(_options.ProductFileOptions.FileName);
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
                    csv.TryGetField(2, out string name);
                    csv.TryGetField(9, out string shippingTime);
                    if (!(shippingTime == $"{shippedIn}h" && !name.ToLower().Contains(productNameNotLike.ToLower())))
                    {
                        continue;
                    }
                    Product product = new Product(
                        id: csv.GetField<int>(0),
                        sku: csv.GetField<string>(1),
                        name: name,
                        ean: csv.GetField<string>(4),
                        producerName: csv.GetField<string>(6),
                        category: csv.GetField<string>(7),
                        isWire: csv.GetField<int>(8),
                        shippingTime: shippingTime,
                        available: csv.GetField<int>(11),
                        isVendor: csv.GetField<int>(16),
                        defaultImage: csv.GetField<string>(18)
                    );
                    products.Add(product);
                }
            }

            return products;
        }

        private async Task<Stream> RequestForDataAndSaveToFileAsync(string fileName)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var fullUrl = $"{_options.BaseUrl}/{fileName}";
                    HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        // TODO: Should I create new type of exception for this situation?
                        throw new Exception();
                    }

                    // TODO: Think about move lines responsible for file writing to separete method and secure situation when there is no directory.
                    if (!Directory.Exists(_options.DirPathForSavingFiles))
                    {
                        Directory.CreateDirectory(_options.DirPathForSavingFiles);
                    }
                    File.WriteAllBytes(Path.Combine(_options.DirPathForSavingFiles, fileName), await response.Content.ReadAsByteArrayAsync());

                    return await response.Content.ReadAsStreamAsync();
                }
                catch (Exception)
                {
                    throw;
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

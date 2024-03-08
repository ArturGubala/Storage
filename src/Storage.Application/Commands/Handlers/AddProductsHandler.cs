using Storage.Application.Abstractions;
using Storage.Application.Exceptions;
using Storage.Core.Repositories;

namespace Storage.Application.Commands.Handlers
{
    public sealed class AddProductsHandler : ICommandHandler<AddProducts>
    {
        private readonly ISourceDataRepository _sourceDataRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IPriceRepository _priceRepository;

        public AddProductsHandler(ISourceDataRepository sourceDataRepository, IProductRepository productRepository,
                                  IInventoryRepository inventoryRepository, IPriceRepository priceRepository)
        {
            _sourceDataRepository = sourceDataRepository;
            _priceRepository = priceRepository;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task HandleAsync(AddProducts command)
        {
            var (shippedIn, productNameNotLike) = command;

            if (shippedIn != 24 && shippedIn != 48 && shippedIn != 72)
            {
                throw new ShippedInValueNotAllowedException();
            }

            var products = await _sourceDataRepository.GetProductsAsync(shippedIn, productNameNotLike);
            var inventory = await _sourceDataRepository.GetInventoryAsync(shippedIn);
            var prices = await _sourceDataRepository.GetPricesAsync(products);

            await _productRepository.AddManyAsync(products);
            await _inventoryRepository.AddManyAsync(inventory);
            await _priceRepository.AddManyAsync(prices);
        }
    }
}

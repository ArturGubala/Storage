using Storage.Application.Abstractions;
using Storage.Core.Repositories;

namespace Storage.Application.Commands.Handlers
{
    public sealed class AddProductsHandler : ICommandHandler<AddProducts>
    {
        private readonly ISourceDataRepository _sourceDataRepository;

        public AddProductsHandler(ISourceDataRepository sourceDataRepository)
        {
            _sourceDataRepository = sourceDataRepository;
        }

        public async Task HandleAsync(AddProducts command)
        {
            var (shippedIn, productNameLike) = command;
            var products = await _sourceDataRepository.GetProductsAsync(shippedIn, productNameLike);
            var inventory = await _sourceDataRepository.GetInventoryAsync(products);
            var prices = await _sourceDataRepository.GetPricesAsync(products);
        }
    }
}

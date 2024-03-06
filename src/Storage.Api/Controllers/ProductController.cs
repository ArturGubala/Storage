using Microsoft.AspNetCore.Mvc;
using Storage.Application.Abstractions;
using Storage.Application.Commands;

namespace Storage.Api.Controller
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ICommandHandler<AddProducts> _addProductsHandler;

        public ProductController(ICommandHandler<AddProducts> addProductsHandler)
        {
            _addProductsHandler = addProductsHandler;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromQuery] int shippedIn, [FromQuery] string productNameLike, AddProducts command)
        {
            await _addProductsHandler.HandleAsync(command with { ShippingIn = shippedIn, ProductNameLike = productNameLike});
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Storage.Application.Abstractions;
using Storage.Application.Commands;
using Storage.Application.DTO;
using Storage.Application.Queries;

namespace Storage.Api.Controller
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ICommandHandler<AddProducts> _addProductsHandler;
        private readonly IQueryHandler<GetProduct, ProductDto> _getProductHandler;

        public ProductController(ICommandHandler<AddProducts> addProductsHandler, IQueryHandler<GetProduct, ProductDto> getProductHandler)
        {
            _addProductsHandler = addProductsHandler;
            _getProductHandler = getProductHandler;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromQuery] int shippedIn, [FromQuery] string productNameLike, AddProducts command)
        {
            await _addProductsHandler.HandleAsync(command with { ShippingIn = shippedIn, ProductNameLike = productNameLike});
            return NoContent();
        }

        [HttpGet("{sku}")]
        public async Task<ActionResult> Get([FromRoute] string sku)
        {
            var product = await _getProductHandler.HandleAsync(new GetProduct { Sku = sku });
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}

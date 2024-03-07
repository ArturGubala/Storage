using Storage.Application.Abstractions;
using Storage.Application.DTO;

namespace Storage.Application.Queries
{
    public class GetProduct : IQuery<ProductDto>
    {
        public string Sku { get; set; }
    }
}

using Storage.Application.Abstractions;

namespace Storage.Application.Commands
{
    public sealed record AddProducts(int ShippingIn, string productNameLike) : ICommand;
}

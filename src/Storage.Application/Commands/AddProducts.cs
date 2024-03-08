using Storage.Application.Abstractions;

namespace Storage.Application.Commands
{
    public sealed record AddProducts(int ShippingIn, string ProductNameNotLike) : ICommand;
}

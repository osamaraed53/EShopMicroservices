using BuildingBlocks.Exceptions;

namespace Basket.Api.Exception; 

public class BasketNotFoundException(string userName) : NotFoundException("Basket",userName)
{
}

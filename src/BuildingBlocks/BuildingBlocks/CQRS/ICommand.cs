using MediatR;

namespace BuildingBlocks.CQRS;

// Unit Is Void Type in MediatR
public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

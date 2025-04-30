using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptor;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{

    //private readonly IMediator _mediator = mediator;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {

        DispatchDomainEvent(eventData.Context).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);

    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvent(eventData.Context);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }


    public async Task DispatchDomainEvent(DbContext context)
    {
        if (context == null) return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ToList().ForEach(a => a.ClearDomainEvent());


        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }



}

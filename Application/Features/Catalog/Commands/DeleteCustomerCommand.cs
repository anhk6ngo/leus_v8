namespace Leus.Application.Features.Catalog.Commands;

public class DeleteCustomerCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteCustomerCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<DeleteCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CCustomer>().Entities.Where(w => w.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        cache.Remove(Caches.GetAllCustomerCacheKey);
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
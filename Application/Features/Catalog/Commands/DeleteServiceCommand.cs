namespace Leus.Application.Features.Catalog.Commands;

public class DeleteServiceCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteServiceCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<DeleteServiceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CService>().Entities.Where(w => w.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        cache.Remove(Caches.GetAllServiceCacheKey);
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
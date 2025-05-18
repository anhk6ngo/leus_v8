namespace Leus.Application.Features.Catalog.Commands;

public class DeleteStoreAddressCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteStoreAddressCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<DeleteStoreAddressCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteStoreAddressCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CStoreAddress>().Entities.Where(w => w.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        cache.Remove(Caches.GetAllStoreAddressCacheKey);
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
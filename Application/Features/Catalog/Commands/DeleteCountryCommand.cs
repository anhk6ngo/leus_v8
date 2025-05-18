namespace Leus.Application.Features.Catalog.Commands;

public class DeleteCountryCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteCountryCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<DeleteCountryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CCountry>().Entities.Where(w => w.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        cache.Remove(Caches.GetAllCountryCacheKey);
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
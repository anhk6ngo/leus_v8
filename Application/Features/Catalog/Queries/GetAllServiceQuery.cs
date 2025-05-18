namespace Leus.Application.Features.Catalog.Queries;

public class GetAllServiceQuery : IRequest<List<CServiceDto>>
{
}

internal class GetAllServiceQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<GetAllServiceQuery, List<CServiceDto>>
{
    public async Task<List<CServiceDto>> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
    {
        Task<List<CServiceDto>> ExpResult() =>
            unitOfWork.RepositoryNew<CService>()
                .Entities.Where(w => w.IsActive == true)
                .ProjectToType<CServiceDto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        return await cache.GetOrAddAsync(Caches.GetAllServiceCacheKey, (Func<Task<List<CServiceDto>>>)ExpResult,
            TimeSpan.FromMinutes(15));
    }
}
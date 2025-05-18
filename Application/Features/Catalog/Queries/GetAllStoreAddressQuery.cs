namespace Leus.Application.Features.Catalog.Queries;

public class GetAllStoreAddressQuery : IRequest<List<CStoreAddressDto>>
{
}

internal class GetAllStoreAddressQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<GetAllStoreAddressQuery, List<CStoreAddressDto>>
{
    public async Task<List<CStoreAddressDto>> Handle(GetAllStoreAddressQuery request, CancellationToken cancellationToken)
    {
        Task<List<CStoreAddressDto>> ExpResult() =>
            unitOfWork.RepositoryNew<CStoreAddress>()
                .Entities.Where(w => w.IsActive == true)
                .ProjectToType<CStoreAddressDto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        return await cache.GetOrAddAsync(Caches.GetAllStoreAddressCacheKey, (Func<Task<List<CStoreAddressDto>>>?)ExpResult,
            TimeSpan.FromMinutes(15));
    }
}
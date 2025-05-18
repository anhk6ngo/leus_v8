namespace Leus.Application.Features.Data.Queries;

public class GetAllSellingPriceQuery : IRequest<List<CPriceDto>>
{
}

internal class GetAllSellingPriceQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<GetAllSellingPriceQuery, List<CPriceDto>>
{
    public async Task<List<CPriceDto>> Handle(GetAllSellingPriceQuery request, CancellationToken cancellationToken)
    {
        var result = await cache.GetOrAddAsync(Caches.GetAllPriceCacheKey, (Func<Task<List<CPriceDto>>>?)ExpResult,
            TimeSpan.FromMinutes(15));
        return result;
        Task<List<CPriceDto>> ExpResult() =>
            unitOfWork.RepositoryNew<CPrice>()
                .Entities.ProjectToType<CPriceDto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }
}
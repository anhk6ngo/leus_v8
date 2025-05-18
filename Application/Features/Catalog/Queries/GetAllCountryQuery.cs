namespace Leus.Application.Features.Catalog.Queries;

public class GetAllCountryQuery : IRequest<List<CCountryDto>>
{
}

internal class GetAllCountryQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<GetAllCountryQuery, List<CCountryDto>>
{
    public async Task<List<CCountryDto>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        Task<List<CCountryDto>> ExpResult() =>
            unitOfWork.RepositoryNew<CCountry>()
                .Entities.Where(w => w.IsActive == true)
                .ProjectToType<CCountryDto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        return await cache.GetOrAddAsync(Caches.GetAllCountryCacheKey, (Func<Task<List<CCountryDto>>>?)ExpResult,
            TimeSpan.FromMinutes(15));
    }
}
namespace Leus.Application.Features.Catalog.Queries;

public class GetAllCustomerQuery : IRequest<List<CCustomerDto>>
{
}

internal class GetAllCustomerQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork, IAppCache cache)
    : IRequestHandler<GetAllCustomerQuery, List<CCustomerDto>>
{
    public async Task<List<CCustomerDto>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        Task<List<CCustomerDto>> ExpResult() =>
            unitOfWork.RepositoryNew<CCustomer>()
                .Entities.Where(w => w.IsActive == true)
                .ProjectToType<CCustomerDto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        return await cache.GetOrAddAsync(Caches.GetAllCustomerCacheKey, (Func<Task<List<CCustomerDto>>>?)ExpResult,
            TimeSpan.FromMinutes(15));
    }
}
namespace Leus.Application.Features.Catalog.Queries;

public class GetAllUserIdQuery : IRequest<List<string>>
{
}

internal class GetAllUserIdQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetAllUserIdQuery, List<string>>
{
    public async Task<List<string>> Handle(GetAllUserIdQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.RepositoryAgg<UserBalance>().Entities
            .AsNoTracking()
            .Select(s=> $"{s.Id}")
            .ToListAsync(cancellationToken);
    }
}
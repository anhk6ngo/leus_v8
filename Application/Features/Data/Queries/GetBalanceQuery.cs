namespace LeUs.Application.Features.Data.Queries;

public class GetBalanceQuery : IRequest<List<UserBalance>>
{
}

internal class GetBalanceQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetBalanceQuery, List<UserBalance>>
{
    public async Task<List<UserBalance>> Handle(GetBalanceQuery request,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.RepositoryAgg<UserBalance>().Entities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
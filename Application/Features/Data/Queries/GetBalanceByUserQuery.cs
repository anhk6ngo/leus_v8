namespace LeUs.Application.Features.Data.Queries;

public class GetBalanceByUserQuery : IRequest<UserBalanceDto?>
{
    public string? UserId { get; set; }
}

internal class GetBalanceByUserQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetBalanceByUserQuery, UserBalanceDto?>
{
    public async Task<UserBalanceDto?> Handle(GetBalanceByUserQuery request,
        CancellationToken cancellationToken)
    {
        var id = $"{request.UserId}".ToGuid();
        var result = await unitOfWork.RepositoryAgg<UserBalance>().Entities
            .AsNoTracking()
            .ProjectToType<UserBalanceDto>()
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        return result ?? new UserBalanceDto();
    }
}
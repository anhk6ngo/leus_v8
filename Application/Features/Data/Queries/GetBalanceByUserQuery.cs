namespace LeUs.Application.Features.Data.Queries;

public class GetBalanceByUserQuery : IRequest<UserBalanceDto?>
{
    public string? UserId { get; set; }
}

internal class GetBalanceByUserQueryHandler(PortalContext context)
    : IRequestHandler<GetBalanceByUserQuery, UserBalanceDto?>
{
    public async Task<UserBalanceDto?> Handle(GetBalanceByUserQuery request,
        CancellationToken cancellationToken)
    {
        var id = $"{request.UserId}".ToGuid();
        if(id == Guid.Empty) return new UserBalanceDto();
        var result = await _getBalanceByUserId(context, id);
        return result == null ? new UserBalanceDto() : result.Adapt<UserBalanceDto>();
    }
    
    private static readonly Func<PortalContext, Guid, Task<UserBalance?>> _getBalanceByUserId =
        EF.CompileAsyncQuery((PortalContext context, Guid id) =>
            context.Set<UserBalance>().AsNoTracking().FirstOrDefault(n => n.Id == id));
}
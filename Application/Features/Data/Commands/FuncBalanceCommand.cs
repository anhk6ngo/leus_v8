namespace LeUs.Application.Features.Data.Commands;

public class FuncBalanceCommand : IRequest<bool>
{
    public BalanceRequest Input { get; set; } = null!;
}

internal class FuncBalanceCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<FuncBalanceCommand, bool>
{
    public async Task<bool> Handle(FuncBalanceCommand request, CancellationToken cancellationToken)
    {
        var id = $"{request.Input.UserId}".ToGuid();
        var dAmount = request.Input.Amount * (request.Input.Action == ActionCommandType.Add ? 1 : -1);
        if (request.Input.Action == ActionCommandType.GetData)
        {
            var blnCheck = await unitOfWork.RepositoryAgg<UserBalance>()
                .Entities.AnyAsync(w=>w.Id == id && w.Amount >= request.Input.Amount, cancellationToken);
            if (!blnCheck) return false;
            await unitOfWork.RepositoryAgg<UserBalance>().Entities.Where(w => w.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(b => b.Amount, b=>b.Amount + dAmount),
                    cancellationToken);
            return true;
        }
        await unitOfWork.RepositoryAgg<UserBalance>().Entities.Where(w => w.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(b => b.Amount, b=>b.Amount + dAmount),
                cancellationToken);
        return true;
    }
}
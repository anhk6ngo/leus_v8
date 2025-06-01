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
        if (request.Input.Action == ActionCommandType.GetData)
        {
            //Check end-user balance
            var oCheck = await unitOfWork.RepositoryAgg<UserBalance>().SingleAsync(w=>w.Id == id && w.Amount >= request.Input.Amount);
            if (oCheck == null) return false;
            //Reduce end-user balance
            oCheck.Amount -= request.Input.Amount;
            await unitOfWork.RepositoryAgg<UserBalance>().UpdateAsync(oCheck);
            await unitOfWork.Commit(cancellationToken);
            return true;
        }
        var dAmount = request.Input.Amount * (request.Input.Action == ActionCommandType.Add ? 1 : -1);
        await unitOfWork.RepositoryAgg<UserBalance>().Entities.Where(w => w.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(b => b.Amount, b => b.Amount + dAmount),
                cancellationToken);
        return true;
    }
}
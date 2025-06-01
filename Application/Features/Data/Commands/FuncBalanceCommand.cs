using System.Diagnostics;

namespace LeUs.Application.Features.Data.Commands;

public class FuncBalanceCommand : IRequest<double>
{
    public BalanceRequest Input { get; set; } = null!;
}

internal class FuncBalanceCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<FuncBalanceCommand, double>
{
    public async Task<double> Handle(FuncBalanceCommand request, CancellationToken cancellationToken)
    {
        var id = $"{request.Input.UserId}".ToGuid();
        if (request.Input.Action == ActionCommandType.GetData)
        {
            return await unitOfWork.RepositoryAgg<UserBalance>().Entities
                .Where(w=>w.Id == id)
                .AsNoTracking()
                .Select(s=>s.Amount)
                .FirstOrDefaultAsync(cancellationToken);
        }
        var dAmount = request.Input.Amount * (request.Input.Action == ActionCommandType.Add ? 1 : -1);
        var oFind = await unitOfWork.RepositoryAgg<UserBalance>().SingleAsync(w=>w.Id == id);
        if (oFind != null)
        {
            oFind.Amount += dAmount;
            dAmount = oFind.Amount;
            await unitOfWork.RepositoryAgg<UserBalance>().UpdateAsync(oFind);
        }
        else
        {
            oFind = new UserBalance()
            {
                Id = id,
                Amount = dAmount,
            };
            await unitOfWork.RepositoryAgg<UserBalance>().AddAsync(oFind);
        }
        await unitOfWork.Commit(cancellationToken);
        return dAmount.ToRound(2);
    }
}
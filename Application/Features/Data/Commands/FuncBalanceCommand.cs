﻿using System.Diagnostics;

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
            var dAmount1 = await unitOfWork.RepositoryAgg<UserBalance>().Entities
                .Where(w=>w.Id == id)
                .AsNoTracking()
                .Select(s=>s.Amount)
                .FirstOrDefaultAsync(cancellationToken);
            return dAmount1 > request.Input.Amount;
        }
        var dAmount = request.Input.Amount * (request.Input.Action == ActionCommandType.Add ? 1 : -1);
        await unitOfWork.RepositoryAgg<UserBalance>().Entities.Where(w => w.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(b => b.Amount, b => b.Amount + dAmount),
                cancellationToken);

        return true;
    }
}
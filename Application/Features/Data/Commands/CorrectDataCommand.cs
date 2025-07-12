using System.Diagnostics;
using CShipment = LeUs.Domain.Data.CShipment;

namespace Leus.Application.Features.Data.Commands;

public class CorrectDataCommand : IRequest<bool>
{
}

internal class CorrectDataCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<CorrectDataCommand, bool>
{
    public async Task<bool> Handle(CorrectDataCommand command,
        CancellationToken cancellationToken)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var dDate = new DateTime(2025, 6, 1).ToUniversalTime();
        dDate = dDate.AddHours(7);
        var oUPs = new List<CShipment>();
        var oBalances = await unitOfWork.RepositoryAgg<UserBalance>().Entities.ToListAsync(cancellationToken);
        if (oBalances is { Count: 0 }) return true;
        foreach (var uBalance in oBalances)
        {
            var results = await unitOfWork.RepositoryNew<CShipment>().Entities
                .Where(w => w.IsActive && w.CreatedOn >= dDate && w.CreatedBy == $"{uBalance.Id}" && w.ShipmentStatus == 2 && w.Price < w.Cost)
                .Select(s=>new CShipment
                {
                    Id = s.Id,
                    CreatedOn = s.CreatedOn,
                    Price = s.Price,
                    Cost = s.Cost,
                    Remote = s.Remote,
                    ExcessVolumeFee = s.ExcessVolumeFee,
                    ExtraLongFee = s.ExtraLongFee,
                    OverLimitFee = s.OverLimitFee
                })
                .ToListAsync(cancellationToken);
            if (results is { Count: 0 }) continue;
            var dTotal = 0.0;
            results = results.OrderBy(o=>o.CreatedOn).ToList();
            foreach (var item in results)
            {
                var dCost = item.Price.PlusNumber(item.Remote).PlusNumber(item.ExtraLongFee).PlusNumber(item.ExcessVolumeFee).PlusNumber(item.OverLimitFee);
                var dDiff =item.Cost.PlusNumber(dCost * -1);
                if (dDiff < 0) continue;
                dDiff += 0.2;
                item.Remote = item.Remote.PlusNumber(dDiff); 
                dTotal += dDiff;
                oUPs.Add(item);
            }

            if (dTotal > 0.0)
            {
                uBalance.Amount -= dTotal;
                await unitOfWork.RepositoryAgg<UserBalance>().UpdateAsync(uBalance);
            }
        }
        
        if (oUPs is { Count: > 0 })
        {
            foreach (var item in oUPs)
            {
                await unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => w.Id == item.Id)
                    .ExecuteUpdateAsync(x => x.SetProperty(b => b.Remote, item.Remote),
                        cancellationToken);
            }
            await unitOfWork.Commit(cancellationToken);
        }
        stopWatch.Stop();
        Console.WriteLine(stopWatch.ElapsedMilliseconds);
        return true;
    }
}
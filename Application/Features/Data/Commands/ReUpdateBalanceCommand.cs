using System.Diagnostics;

namespace LeUs.Application.Features.Data.Commands;

public class ReUpdateBalanceCommand : IRequest<bool>
{
}

internal class ReUpdateBalanceCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<ReUpdateBalanceCommand, bool>
{
    public async Task<bool> Handle(ReUpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var dFrom = DateTime.Today.AddDays(-1).ToUtc();
        var dTo = dFrom.AddDays(1).AddSeconds(-1);
        var oIds = await unitOfWork.RepositoryNew<CShipment>()
            .Entities.AsNoTracking()
            .Where(w => w.CreatedOn >= dFrom && w.CreatedOn <= dTo && w.CreatedBy != null && w.ShipmentStatus == 2)
            .GroupBy(g => g.CreatedBy)
            .Select(s => new
            {
                id = s.Key,
            }).ToListAsync(cancellationToken);
        if (oIds is { Count: 0 }) return true;
        var tmpId = oIds.Select(o => $"{o.id}").ToList();
        var dStart = new DateTime(2025, 1, 1).ToUtc();
        var oLabels = await unitOfWork.RepositoryNew<CShipment>()
            .Entities.AsNoTracking()
            .Where(w => w.CreatedOn >= dStart && w.ShipmentStatus == 2 && tmpId.Contains(w.CreatedBy!))
            .GroupBy(g => g.CreatedBy)
            .Select(s => new
            {
                Id = s.Key,
                p = s.Sum(sm => sm.Price),
                r = s.Sum(sm => sm.Remote),
                ev = s.Sum(sm => sm.ExcessVolumeFee),
                ol = s.Sum(sm => sm.OverLimitFee),
                el = s.Sum(sm => sm.ExtraLongFee),
            }).ToListAsync(cancellationToken);
        tmpId = oLabels.Select(s => $"{s.Id}").ToList();
        var oTops = await unitOfWork.RepositoryNew<CTopUp>().Entities.AsNoTracking()
            .Where(w => w.IsActive && w.RequestDate != null && w.Status == 2 && tmpId.Contains(w.UserId!))
            .GroupBy(g => g.UserId)
            .Select(s => new
            {
                Id = s.Key,
                Amount = s.Sum(sm => sm.ApproveAmount)
            }).ToListAsync(cancellationToken);
        if (oTops is { Count: 0 }) return true;
        foreach (var oTop in oTops)
        {
            var oFind = oLabels.FirstOrDefault(w => w.Id == oTop.Id);
            if (oFind == null) continue;
            var dRemain = oTop.Amount.PlusNumber(oFind.p.PlusNumber(oFind.r).PlusNumber(oFind.ev).PlusNumber(oFind.ol)
                .PlusNumber(oFind.el) * -1);
            var id = oTop.Id.ToGuid();
            await unitOfWork.RepositoryAgg<UserBalance>().Entities.Where(w => w.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(b => b.Amount, dRemain),
                    cancellationToken);
        }
        stopwatch.Stop();
        return true;
    }
}
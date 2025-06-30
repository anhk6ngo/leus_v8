namespace Leus.Application.Features.Data.Queries;

public class GetSumShipmentByUserQuery : IRequest<List<SumShipmentResponse>>
{
    public GetReportRequest Input { get; set; } = null!;
}

internal class GetSumShipmentByUserQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetSumShipmentByUserQuery, List<SumShipmentResponse>>
{
    public async Task<List<SumShipmentResponse>> Handle(GetSumShipmentByUserQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.Input.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var oFilter = PredicateBuilder.True<CShipment>();
        oFilter = oFilter.And(w => w.CreatedOn >= oDateRange.dFrom
                                   && w.CreatedOn <= oDateRange.dTo);
        if (request.Input.UserId.NotIsNullOrEmpty())
        {
            oFilter = oFilter.And(w => w.CreatedBy == request.Input.UserId);
        }
        var result = await unitOfWork.RepositoryNew<CShipment>().Entities
            .AsNoTracking()
            .Where(oFilter)
            .Select(s => new SumShipmentResponse
            {
                TransDate = new DateTime(s.CreatedOn.Year, s.CreatedOn.Month, s.CreatedOn.Day),
                CustomerId = s.CustomerId,
                Amount = s.Price ?? 0,
                Remote = s.Remote ?? 0,
                Cost = s.Cost ?? 0,
                ExcessVolume = s.ExcessVolumeFee ?? 0,
                ExtraLong = s.ExtraLongFee ?? 0,
                OverLimit = s.OverLimitFee ?? 0,
                ActiveNo = s.IsActive ? 1 : 0,
                DeActiveNo = s.IsActive ? 0 : 1,
                Status = s.ShipmentStatus,
                GenerateLabelNo = s.ShipmentStatus == 2 ? 1 : 0,
                CancelLabelNo = s.ShipmentStatus == 3 ? 1 : 0,
                ServiceCode = s.ServiceCode,
            })
            .GroupBy(g => new
            {
                g.ServiceCode,
                g.TransDate,
                g.Status,
                g.CustomerId,
            }).Select(s => new SumShipmentResponse()
            {
                ServiceCode = s.Key.ServiceCode,
                TransDate = s.Key.TransDate,
                Status = s.Key.Status,
                CustomerId = s.Key.CustomerId,
                Amount = s.Sum(sm => sm.Amount),
                Cost = s.Sum(sm => sm.Cost),
                Remote = s.Sum(sm => sm.Remote),
                ExtraLong = s.Sum(sm => sm.ExtraLong),
                ExcessVolume = s.Sum(sm => sm.ExcessVolume),
                OverLimit = s.Sum(sm => sm.OverLimit),
                ActiveNo = s.Sum(sm => sm.ActiveNo),
                DeActiveNo = s.Sum(sm => sm.DeActiveNo),
                GenerateLabelNo = s.Sum(sm => sm.GenerateLabelNo),
                CancelLabelNo = s.Sum(sm => sm.CancelLabelNo),
            }).ToListAsync(cancellationToken);
        return result;
    }
}
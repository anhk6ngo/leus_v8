using System.Diagnostics;

namespace Leus.Application.Features.Data.Queries;

public class GetSumShipmentByUserQuery : IRequest<List<SumShipmentResponse>>
{
    public GetReportRequest Input { get; set; } = null!;
}

internal class GetSumShipmentByUserQueryHandler(PortalContext context)
    : IRequestHandler<GetSumShipmentByUserQuery, List<SumShipmentResponse>>
{
    public async Task<List<SumShipmentResponse>> Handle(GetSumShipmentByUserQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.Input.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var result = new List<SumShipmentResponse>();
        if (request.Input.UserId.NotIsNullOrEmpty())
        {
            await foreach (var item in GetDataByUserAsync(context, oDateRange.dFrom, oDateRange.dTo,
                               $"{request.Input.UserId}"))
            {
                result.Add(item);
            }

            return result;
        }

        await foreach (var item in GetAllDataAsync(context, oDateRange.dFrom, oDateRange.dTo))
        {
            result.Add(item);
        }

        return result;
    }

    private static readonly Func<PortalContext, DateTime, DateTime, IAsyncEnumerable<SumShipmentResponse>>
        GetAllDataAsync =
            EF.CompileAsyncQuery((PortalContext context, DateTime dFrom, DateTime dTo) =>
                context.Set<CShipment>().Where(n => n.CreatedOn >= dFrom && n.CreatedOn <= dTo)
                    .GroupBy(g => new
                    {
                        g.ServiceCode,
                        g.CreatedOn.Date,
                        g.ShipmentStatus,
                        g.CustomerId,
                        g.Consignee!.State
                    }).Select(s => new SumShipmentResponse()
                    {
                        ServiceCode = s.Key.ServiceCode,
                        TransDate = s.Key.Date,
                        Status = s.Key.ShipmentStatus,
                        CustomerId = s.Key.CustomerId,
                        State = s.Key.State,
                        Amount = s.Sum(sm => sm.Price ?? 0),
                        Cost = s.Sum(sm => sm.Cost ?? 0),
                        Remote = s.Sum(sm => sm.Remote ?? 0),
                        ExtraLong = s.Sum(sm => sm.ExtraLongFee ?? 0),
                        ExcessVolume = s.Sum(sm => sm.ExcessVolumeFee ?? 0),
                        OverLimit = s.Sum(sm => sm.OverLimitFee ?? 0),
                        ActiveNo = s.Sum(sm => sm.IsActive ? 1 : 0),
                        DeActiveNo = s.Sum(sm => sm.IsActive ? 0 : 1),
                        GenerateLabelNo = s.Sum(sm => sm.ShipmentStatus == 2 ? 1 : 0),
                        CancelLabelNo = s.Sum(sm => sm.ShipmentStatus == 3 ? 1 : 0),
                    }));

    private static readonly Func<PortalContext, DateTime, DateTime, string, IAsyncEnumerable<SumShipmentResponse>>
        GetDataByUserAsync =
            EF.CompileAsyncQuery((PortalContext context, DateTime dFrom, DateTime dTo, string userId) =>
                context.Set<CShipment>().Where(w => w.CreatedOn >= dFrom && w.CreatedOn <= dTo && w.CreatedBy == userId)
                    .GroupBy(g => new
                    {
                        g.ServiceCode,
                        g.CreatedOn.Date,
                        g.ShipmentStatus,
                        g.CustomerId,
                        g.Consignee!.State
                    }).Select(s => new SumShipmentResponse()
                    {
                        ServiceCode = s.Key.ServiceCode,
                        TransDate = s.Key.Date,
                        Status = s.Key.ShipmentStatus,
                        CustomerId = s.Key.CustomerId,
                        State = s.Key.State,
                        Amount = s.Sum(sm => sm.Price ?? 0),
                        Cost = s.Sum(sm => sm.Cost ?? 0),
                        Remote = s.Sum(sm => sm.Remote ?? 0),
                        ExtraLong = s.Sum(sm => sm.ExtraLongFee ?? 0),
                        ExcessVolume = s.Sum(sm => sm.ExcessVolumeFee ?? 0),
                        OverLimit = s.Sum(sm => sm.OverLimitFee ?? 0),
                        ActiveNo = s.Sum(sm => sm.IsActive ? 1 : 0),
                        DeActiveNo = s.Sum(sm => sm.IsActive ? 0 : 1),
                        GenerateLabelNo = s.Sum(sm => sm.ShipmentStatus == 2 ? 1 : 0),
                        CancelLabelNo = s.Sum(sm => sm.ShipmentStatus == 3 ? 1 : 0),
                    }));
}
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
                                   && w.CreatedOn <= oDateRange.dTo && w.CreatedBy == request.Input.UserId);
        var result = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(oFilter)
            .AsNoTracking()
            .Select(s => new SumShipmentResponse
            {
                TransDate = new DateTime(s.CreatedOn.Year, s.CreatedOn.Month, s.CreatedOn.Day),
                Amount = s.Price ?? 0,
                ActiveNo = s.IsActive ? 1 : 0,
                DeActiveNo = s.IsActive ? 0 : 1,
                GenerateLabelNo = s.ShipmentStatus == 2 ? 1 : 0,
                ServiceCode = s.ServiceCode,
            })
            .GroupBy(g => new
            {
                g.ServiceCode,
                g.TransDate
            }).Select(s => new SumShipmentResponse()
            {
                ServiceCode = s.Key.ServiceCode,
                TransDate = s.Key.TransDate,
                Amount = s.Sum(s => s.Amount),
                ActiveNo = s.Sum(s => s.ActiveNo),
                DeActiveNo = s.Sum(s => s.DeActiveNo),
                GenerateLabelNo = s.Sum(s => s.GenerateLabelNo),
            }).ToListAsync(cancellationToken);
        return result;
    }
}
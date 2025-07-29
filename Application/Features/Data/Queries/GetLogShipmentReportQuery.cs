namespace Leus.Application.Features.Data.Queries;

public class GetLogShipmentReportQuery : IRequest<List<LogResult>>
{
    public string? DateRange { get; set; }
    public int TypeReport { get; set; } = 0;
}

internal class GetLogShipmentReportQueryHandler(PortalContext context)
    : IRequestHandler<GetLogShipmentReportQuery, List<LogResult>>
{
    public async Task<List<LogResult>> Handle(GetLogShipmentReportQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        return await context.Set<CHistoryLabel>()
            .AsNoTracking()
            .GetLogShipment(oDateRange.dFrom, oDateRange.dTo, request.TypeReport == 0)
            .ToListAsync(cancellationToken);
    }
}
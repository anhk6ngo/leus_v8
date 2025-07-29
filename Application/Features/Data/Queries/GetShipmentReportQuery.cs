namespace Leus.Application.Features.Data.Queries;

public class GetShipmentReportQuery : IRequest<List<CShipmentReport>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int TypeReport { get; set; } = 0;
}

internal class GetShipmentReportQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetShipmentReportQuery, List<CShipmentReport>>
{
    public async Task<List<CShipmentReport>> Handle(GetShipmentReportQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var iStatus = request.TypeReport == 0 ? 2 : 3;
        var result = await unitOfWork.RepositoryNew<CShipment>().Entities.AsNoTracking()
            .ApplyLabelDate(oDateRange.dFrom, oDateRange.dTo, iStatus, request.UserId)
            .ProjectToType<CShipmentReport>()
            .ToListAsync(cancellationToken);
        return result;
    }
}
namespace Leus.Application.Features.Data.Queries;

public class GetShipmentReportQuery : IRequest<List<CShipmentReport>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
}

internal class GetShipmentReportQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetShipmentReportQuery, List<CShipmentReport>>
{
    public async Task<List<CShipmentReport>> Handle(GetShipmentReportQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var oFilter = PredicateBuilder.True<CShipment>();
        oFilter = oFilter.And(w => w.IsActive && w.CreatedOn >= oDateRange.dFrom
                                             && w.CreatedOn <= oDateRange.dTo && w.ShipmentStatus == 2);
        if (request.UserId.NotIsNullOrEmpty())
        {
            oFilter = oFilter.And(w => w.CreatedBy == request.UserId);
        }
        var result = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(oFilter)
            .ProjectToType<CShipmentReport>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return result;
    }
}
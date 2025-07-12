namespace Leus.Application.Features.Data.Queries;

public class GetShipmentReportQuery : IRequest<List<CShipmentReport>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int TypeReport { get; set; } = 0;
    public string? RefIds { get; set; }
}

internal class GetShipmentReportQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetShipmentReportQuery, List<CShipmentReport>>
{
    public async Task<List<CShipmentReport>> Handle(GetShipmentReportQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var oFilter = PredicateBuilder.True<CShipment>();
        // if (request.TypeReport == -1)
        // {
        //     var ids = $"{request.RefIds}".SplitExt();
        //     oFilter = oFilter.And(w=>ids.Contains(w.ReferenceId));
        //     var tmp = await unitOfWork.RepositoryNew<CShipment>().Entities
        //         .Where(oFilter)
        //         .ProjectToType<CShipmentDto>()
        //         .AsNoTracking()
        //         .ToListAsync(cancellationToken);
        //     if (tmp is not { Count: > 0 }) return [];
        //     foreach (var rqU in tmp.Select(item => TransformHelper.ToUnitedBridge(item)))
        //     {
        //         Console.WriteLine(rqU.ConvertObjectToString());
        //     }
        //     return [];
        // }
        var iStatus = request.TypeReport == 0 ? 2 : 3;
        if (request.TypeReport == 0)
        {
            oFilter = oFilter.And(w => w.CreateLabelDate >= oDateRange.dFrom
                                       && w.CreateLabelDate <= oDateRange.dTo);
        }
        else
        {
            oFilter = oFilter.And(w => w.CancelLabelDate >= oDateRange.dFrom
                                       && w.CancelLabelDate <= oDateRange.dTo);
        }

        oFilter = request.UserId.NotIsNullOrEmpty()
            ? oFilter.And(w => w.ShipmentStatus == iStatus && w.CreatedBy == request.UserId)
            : oFilter.And(w => w.ShipmentStatus == iStatus);

        var result = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(oFilter)
            .ProjectToType<CShipmentReport>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return result;
    }
}
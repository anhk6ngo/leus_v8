namespace Leus.Application.Features.Data.Queries;

public class GetAllShipmentByUserQuery : IRequest<List<CShipmentDto>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int Status { get; set; } = -1;
    public bool IsTimeOut { get; set; } = false;
    public List<string> RefIds { get; set; } = [];
    public List<string> Ref2Ids { get; set; } = [];
    public List<string> TrackIds { get; set; } = [];
}

internal class GetAllShipmentByUserQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetAllShipmentByUserQuery, List<CShipmentDto>>
{
    public async Task<List<CShipmentDto>> Handle(GetAllShipmentByUserQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var oFilter = PredicateBuilder.True<CShipment>();
        if (request.RefIds is { Count: > 0 })
        {
            oFilter = oFilter.And(w => w.IsActive && w.CreatedBy == request.UserId &&
                                       request.RefIds.Contains(w.ReferenceId!));
            if (request.Status >= 0)
            {
                oFilter = oFilter.And(w => w.ShipmentStatus == request.Status);
            }
        }
        else
        {
            if (request.Ref2Ids is { Count: > 0 })
            {
                if (request.TrackIds is { Count: 0 })
                {
                    oFilter = oFilter.And(w =>
                        w.CreatedBy == request.UserId && request.Ref2Ids.Contains(w.ReferenceId2!) &&
                        w.TrackIds != "_");
                }
                else
                {
                    oFilter = oFilter.And(w =>
                        w.CreatedBy == request.UserId && (request.Ref2Ids.Contains(w.ReferenceId2!) &&
                                                          request.TrackIds.Contains(w.TrackIds!)));
                }
            }
            else if (request.TrackIds is { Count: > 0 })
            {
                oFilter = oFilter.And(w =>
                    w.CreatedBy == request.UserId && request.TrackIds.Contains(w.TrackIds!) &&
                    w.ReferenceId2 != "_");
            }
            else
            {
                oFilter = oFilter.And(w => w.CreatedOn >= oDateRange.dFrom
                                                      && w.CreatedOn <= oDateRange.dTo &&
                                                      w.CreatedBy == request.UserId && w.IsActive==true);
            }
        }
        if (request.IsTimeOut)
        {
            oFilter = oFilter.And(w => w.ShipmentStatus == 2);
        }

        var result = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(oFilter)
            .ProjectToType<CShipmentDto>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (request.IsTimeOut)
        {
            result = result.Where(w => w.TotalTime >= 20).ToList();
        }
        return result;
    }
}
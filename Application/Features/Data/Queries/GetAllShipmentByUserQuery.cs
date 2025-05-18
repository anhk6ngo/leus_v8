namespace Leus.Application.Features.Data.Queries;

public class GetAllShipmentByUserQuery : IRequest<List<CShipmentDto>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int Status { get; set; } = -1;
    public List<string> RefIds { get; set; } = [];
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
            oFilter = oFilter.And(w => w.IsActive && w.CreatedOn >= oDateRange.dFrom
                                                  && w.CreatedOn <= oDateRange.dTo && w.CreatedBy == request.UserId);
        }

        var result = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(oFilter)
            .ProjectToType<CShipmentDto>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return result;
    }
}
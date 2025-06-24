namespace Leus.Application.Features.Data.Queries;

public class GetPageShipmentByUserQuery : IRequest<PagedResponseOffset<ShipmentDto>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int Status { get; set; } = -1;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public bool IsTimeOut { get; set; } = false;
    public List<string> RefIds { get; set; } = [];
}

internal class GetPageShipmentByUserQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetPageShipmentByUserQuery, PagedResponseOffset<ShipmentDto>>
{
    public async Task<PagedResponseOffset<ShipmentDto>> Handle(GetPageShipmentByUserQuery request,
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

        var totalRecords = await unitOfWork.RepositoryNew<CShipment>().Entities
            .AsNoTracking()
            .CountAsync(oFilter, cancellationToken);
        var data = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(oFilter)
            .Skip((request.PageNumber - 1)*request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<ShipmentDto>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
            return new PagedResponseOffset<ShipmentDto>(data,
            request.PageNumber,
            request.PageSize,
            totalRecords);
    }
}
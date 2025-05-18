namespace Leus.Application.Features.Data.Queries;

public class GetAllShipmentByRefQuery : IRequest<List<CShipment>>
{
    public List<string> RefIds { get; set; } = [];
    public string? UserId { get; set; }
    public bool GetLabel { get; set; } = true;
}

internal class GetAllShipmentByRefQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetAllShipmentByRefQuery, List<CShipment>>
{
    public async Task<List<CShipment>> Handle(GetAllShipmentByRefQuery request,
        CancellationToken cancellationToken)
    {
        if (request.GetLabel)
        {
            return await unitOfWork.RepositoryNew<CShipment>().Entities
                .Where(w => request.RefIds.Contains(w.ReferenceId!) && w.ShipmentStatus == 2 &&
                            w.CreatedBy == request.UserId)
                .Select(s => new CShipment()
                {
                    Id = s.Id,
                    ApiName = s.ApiName,
                    ApiName1 = s.ApiName1,
                    Labels = s.Labels,
                    ShipmentId = s.ShipmentId,
                    TrackIds = s.TrackIds,
                }).AsNoTracking().ToListAsync(cancellationToken);
        }

        return await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(w => request.RefIds.Contains(w.ReferenceId!) && w.ShipmentStatus == 2 &&
                        w.CreatedBy == request.UserId).ToListAsync(cancellationToken);
    }
}
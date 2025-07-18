﻿namespace Leus.Application.Features.Data.Queries;

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
                .Where(w => request.RefIds.Contains(w.ReferenceId!) &&
                            w.CreatedBy == request.UserId && w.ShipmentStatus == 2)
                .Select(s => new CShipment()
                {
                    Id = s.Id,
                    ApiName = s.ApiName,
                    ApiName1 = s.ApiName1,
                    Labels = s.Labels,
                    ShipmentId = s.ShipmentId,
                    ReferenceId2 = s.ReferenceId2,
                    ServiceCode = s.ServiceCode,
                    TrackIds = s.TrackIds,
                }).AsNoTracking().ToListAsync(cancellationToken);
        }

        return await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(w => request.RefIds.Contains(w.ReferenceId!) &&
                        w.CreatedBy == request.UserId && w.ShipmentStatus == 2)
            .ToListAsync(cancellationToken);
    }
}
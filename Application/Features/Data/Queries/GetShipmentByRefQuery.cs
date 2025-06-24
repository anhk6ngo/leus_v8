namespace Leus.Application.Features.Data.Queries;

public class GetShipmentByRefQuery : IRequest<CShipment?>
{
    public string? RefId { get; set; } = "";
    public string? UserId { get; set; }
    public int ShipmentStatus { get; set; } = 3;
    public Guid Id { get; set; } = Guid.NewGuid();
}

internal class GetShipmentByRefQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetShipmentByRefQuery, CShipment?>
{
    public async Task<CShipment?> Handle(GetShipmentByRefQuery request,
        CancellationToken cancellationToken)
    {
        if (request.ShipmentStatus == 2)
        {
            DateTime? dReset = null;
            await unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => w.Id == request.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(b => b.ShipmentStatus, 2)
                        .SetProperty(b => b.CancelLabelDate, dReset),
                    cancellationToken);
            return null;
        }
        var oShipment = await unitOfWork.RepositoryNew<CShipment>().Entities
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.ReferenceId == request.RefId && w.CreatedBy == request.UserId
                                                                     && w.ShipmentStatus == 2, cancellationToken);
        if (oShipment == null) return oShipment;

        var dCancel = DateTime.UtcNow;
        await unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => w.Id == oShipment.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(b => b.ShipmentStatus, 3)
                    .SetProperty(b => b.CancelLabelDate, dCancel),
                cancellationToken);
        return oShipment;
    }
}
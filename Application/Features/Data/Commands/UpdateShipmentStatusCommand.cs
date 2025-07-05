using CShipment = LeUs.Domain.Data.CShipment;

namespace Leus.Application.Features.Data.Commands;

public class UpdateShipmentStatusCommand : IRequest<bool>
{
    public Guid Id { get; set; } = default!;
    public int Status { get; set; } = 4;
}

internal class UpdateShipmentStatusCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<UpdateShipmentStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateShipmentStatusCommand command,
        CancellationToken cancellationToken)
    {
        await unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => w.Id == command.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(b => b.ShipmentStatus, command.Status),
                cancellationToken);
        return true;
    }
}
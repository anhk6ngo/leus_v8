using CShipment = LeUs.Domain.Data.CShipment;

namespace LeUs.Application.Features.Data.Commands;

public class DeleteShipmentByRefIdCommand : IRequest<bool>
{
    public required List<string> RefIds { get; set; }
}

internal class DeleteShipmentByRefIdCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<DeleteShipmentByRefIdCommand, bool>
{
    public async Task<bool> Handle(DeleteShipmentByRefIdCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CShipment>().Entities
            .Where(w => request.RefIds.Contains(w.ReferenceId!) && w.ShipmentStatus < 2)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        return true;
    }
}
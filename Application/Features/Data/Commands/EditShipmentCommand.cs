using CShipment = LeUs.Domain.Data.CShipment;

namespace Leus.Application.Features.Data.Commands;

public class EditShipmentCommand : IRequest<bool>
{
    public CShipment Data { get; set; } = default!;
}

internal class EditShipmentCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<EditShipmentCommand, bool>
{
    public async Task<bool> Handle(EditShipmentCommand command,
        CancellationToken cancellationToken)
    {
        await unitOfWork.RepositoryNew<CShipment>().UpdateAsync(command.Data);
        await unitOfWork.Commit(cancellationToken);
        return true;
    }
}
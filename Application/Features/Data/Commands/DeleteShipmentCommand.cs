using CShipment = LeUs.Domain.Data.CShipment;

namespace LeUs.Application.Features.Data.Commands;

public class DeleteShipmentCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteShipmentCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<DeleteShipmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteShipmentCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => w.Id == request.Id && w.ShipmentStatus < 2)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
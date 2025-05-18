using CShipment = LeUs.Domain.Data.CShipment;

namespace Leus.Application.Features.Data.Commands;

public class UpdateLabelShipmentCommand : IRequest<bool>
{
    public List<CShipment> Data { get; set; } = [];
    public List<CHistoryLabel> History { get; set; } = [];
    public ActionCommandType Action { get; set; } = ActionCommandType.Edit;
}

internal class UpdateLabelShipmentCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<UpdateLabelShipmentCommand, bool>
{
    public async Task<bool> Handle(UpdateLabelShipmentCommand command,
        CancellationToken cancellationToken)
    {
        if (command.History is { Count: > 0 })
        {
            foreach (var history in command.History)
            {
                await unitOfWork.RepositoryAgg<CHistoryLabel>().AddAsync(history);
            }
        }
        switch (command.Action)
        {
            case ActionCommandType.Edit:
                var oIds = command.Data.Select(x => x.Id).ToList();
                var oUps = unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => oIds.Contains(w.Id)).ToList();
                foreach (var item in oUps)
                {
                    var oFind = command.Data.FirstOrDefault(x => x.Id == item.Id);
                    if (oFind == null) continue;
                    if (item.CreateLabelDate != null)
                    {
                        item.CreateLabelDate = DateTime.UtcNow;
                    }

                    if (command.Action == ActionCommandType.Edit)
                    {
                        item.ShipmentId = oFind.ShipmentId;
                        item.ShipmentStatus = 2;
                        item.Cost = oFind.Cost;
                        if (oFind.Labels is { Count: > 0 })
                        {
                            item.Labels = oFind.Labels;
                            item.TrackIds = oFind.TrackIds;
                        }
                    }
                    else
                    {
                        item.Labels = oFind.Labels;
                        item.TrackIds = oFind.TrackIds;
                    }

                    await unitOfWork.RepositoryNew<CShipment>().UpdateAsync(item);
                }
                await unitOfWork.Commit(cancellationToken);
                break;
            case ActionCommandType.Delete:
                foreach (var item in command.Data)
                {
                    item.Price = 0;
                    item.Remote = 0;
                    item.ShipmentStatus = 3;
                    await unitOfWork.RepositoryNew<CShipment>().UpdateAsync(item);
                }
                await unitOfWork.Commit(cancellationToken);
                break;
        }
        
        return true;
    }
}
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

        if (command.Data is { Count: 0 })
        {
            if (command.History is { Count: > 0 })
            {
                await unitOfWork.Commit(cancellationToken);
            }

            return false;
        }

        switch (command.Action)
        {
            case ActionCommandType.Edit:
            case ActionCommandType.UpdateLabel:
                var oIds = command.Data.Select(x => x.Id).ToList();
                var oUps = unitOfWork.RepositoryNew<CShipment>().Entities.Where(w => oIds.Contains(w.Id)).ToList();
                foreach (var item in oUps)
                {
                    var oFind = command.Data.FirstOrDefault(x => x.Id == item.Id);
                    if (oFind == null) continue;
                    if (command.Action == ActionCommandType.Edit)
                    {
                        item.ShipmentId = oFind.ShipmentId;
                        item.CreateLabelDate = oFind.CreateLabelDate;
                        item.ShipmentStatus = 2;
                        item.TotalTime = oFind.TotalTime;
                        item.Cost = oFind.Cost;
                        item.Remote = oFind.Remote;
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
                    //item.Price = 0;
                    //item.Remote = 0;
                    item.ShipmentStatus = 3;
                    item.CancelLabelDate = DateTime.UtcNow;
                    await unitOfWork.RepositoryNew<CShipment>().UpdateAsync(item);
                }
                await unitOfWork.Commit(cancellationToken);
                break;
        }
        return true;
    }
}
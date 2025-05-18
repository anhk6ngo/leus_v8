namespace Leus.Application.Features.Data.Commands;

public class AddEditSellingPriceCommand : IRequest<Result<Guid>>
{
    public AddEditDataRequest<CPriceDto> Request { get; set; } = default!;
}

internal class AddEditSellingPriceCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<AddEditSellingPriceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEditSellingPriceCommand command,
        CancellationToken cancellationToken)
    {
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
                var oNewItem = command.Request.Data.Adapt<CPrice>();
                oNewItem.Zones ??= [];
                await unitOfWork.RepositoryNew<CPrice>().AddAsync(oNewItem);
                await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllPriceCacheKey);
                return await Result<Guid>.SuccessAsync(oNewItem.Id, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CPrice>().GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    command.Request.Data.Adapt(currentItem);
                    await unitOfWork.RepositoryNew<CPrice>().UpdateAsync(currentItem);
                    await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllPriceCacheKey);
                    return await Result<Guid>.SuccessAsync(currentItem.Id, "The item updated");
                }

                break;
        }

        return await Result<Guid>.FailAsync("Not found the item");
    }
}
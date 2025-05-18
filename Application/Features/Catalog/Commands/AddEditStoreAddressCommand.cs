namespace Leus.Application.Features.Catalog.Commands;

public class AddEditStoreAddressCommand : IRequest<Result<Guid>>
{
    public AddEditDataRequest<CStoreAddressDto> Request { get; set; } = default!;
}

internal class AddEditStoreAddressCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<AddEditStoreAddressCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEditStoreAddressCommand command, CancellationToken cancellationToken)
    {
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
                var oNewItem = command.Request.Data.Adapt<CStoreAddress>();
                await unitOfWork.RepositoryNew<CStoreAddress>().AddAsync(oNewItem);
                await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllStoreAddressCacheKey);
                return await Result<Guid>.SuccessAsync(oNewItem.Id, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CStoreAddress>().GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    command.Request.Data.Adapt(currentItem);
                    await unitOfWork.RepositoryNew<CStoreAddress>().UpdateAsync(currentItem);
                    await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllStoreAddressCacheKey);
                    return await Result<Guid>.SuccessAsync(currentItem.Id, "The item updated");
                }

                break;
        }

        return await Result<Guid>.FailAsync("Not found the item");
    }
}
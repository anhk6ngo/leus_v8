namespace Leus.Application.Features.Catalog.Commands;

public class AddEditAddressCommand : IRequest<Result<Guid>>
{
    public AddEditDataRequest<CAddressDto> Request { get; set; } = default!;
}

internal class AddEditAddressCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<AddEditAddressCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEditAddressCommand command, CancellationToken cancellationToken)
    {
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
                var oNewItem = command.Request.Data.Adapt<CAddress>();
                await unitOfWork.RepositoryNew<CAddress>().AddAsync(oNewItem);
                await unitOfWork.Commit(cancellationToken);
                return await Result<Guid>.SuccessAsync(oNewItem.Id, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CAddress>().GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    command.Request.Data.Adapt(currentItem);
                    await unitOfWork.RepositoryNew<CAddress>().UpdateAsync(currentItem);
                    await unitOfWork.Commit(cancellationToken);
                    return await Result<Guid>.SuccessAsync(currentItem.Id, "The item updated");
                }

                break;
        }

        return await Result<Guid>.FailAsync("Not found the item");
    }
}
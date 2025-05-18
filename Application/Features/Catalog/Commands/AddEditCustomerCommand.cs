namespace Leus.Application.Features.Catalog.Commands;

public class AddEditCustomerCommand : IRequest<Result<Guid>>
{
    public AddEditDataRequest<CCustomerDto> Request { get; set; } = default!;
}

internal class AddEditCustomerCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<AddEditCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEditCustomerCommand command, CancellationToken cancellationToken)
    {
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
                var oNewItem = command.Request.Data.Adapt<CCustomer>();
                await unitOfWork.RepositoryNew<CCustomer>().AddAsync(oNewItem);
                await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllCustomerCacheKey);
                return await Result<Guid>.SuccessAsync(oNewItem.Id, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CCustomer>().GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    command.Request.Data.Adapt(currentItem);
                    await unitOfWork.RepositoryNew<CCustomer>().UpdateAsync(currentItem);
                    await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllCustomerCacheKey);
                    return await Result<Guid>.SuccessAsync(currentItem.Id, "The item updated");
                }

                break;
        }

        return await Result<Guid>.FailAsync("Not found the item");
    }
}
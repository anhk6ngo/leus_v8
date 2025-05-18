namespace Leus.Application.Features.Catalog.Commands;

public class AddEditCountryCommand : IRequest<Result<Guid>>
{
    public AddEditDataRequest<CCountryDto> Request { get; set; } = default!;
}

internal class AddEditCountryCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<AddEditCountryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEditCountryCommand command, CancellationToken cancellationToken)
    {
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
                var oNewItem = command.Request.Data.Adapt<CCountry>();
                await unitOfWork.RepositoryNew<CCountry>().AddAsync(oNewItem);
                await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllCountryCacheKey);
                return await Result<Guid>.SuccessAsync(oNewItem.Id, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CCountry>().GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    command.Request.Data.Adapt(currentItem);
                    await unitOfWork.RepositoryNew<CCountry>().UpdateAsync(currentItem);
                    await unitOfWork.CommitAndRemoveCache(cancellationToken, Caches.GetAllCountryCacheKey);
                    return await Result<Guid>.SuccessAsync(currentItem.Id, "The item updated");
                }

                break;
        }

        return await Result<Guid>.FailAsync("Not found the item");
    }
}
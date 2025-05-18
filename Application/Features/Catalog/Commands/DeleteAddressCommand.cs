namespace Leus.Application.Features.Catalog.Commands;

public class DeleteAddressCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteAddressCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<DeleteAddressCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var currentItem = await unitOfWork.RepositoryNew<CAddress>().GetByIdAsync(request.Id);
        if (currentItem == null) return await Result<Guid>.FailAsync("Not found the item");
        currentItem.IsActive = false;
        await unitOfWork.RepositoryNew<CAddress>().UpdateAsync(currentItem);
        await unitOfWork.Commit(cancellationToken);
        return await Result<Guid>.SuccessAsync(currentItem.Id, "The item deleted");
    }
}
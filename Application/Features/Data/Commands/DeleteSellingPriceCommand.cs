namespace LeUs.Application.Features.Data.Commands;

public class DeleteSellingPriceCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteSellingPriceCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<DeleteSellingPriceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteSellingPriceCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CPrice>().Entities.Where(w => w.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
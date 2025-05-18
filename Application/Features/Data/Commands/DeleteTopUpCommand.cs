using CTopUp = LeUs.Domain.Data.CTopUp;

namespace LeUs.Application.Features.Data.Commands;

public class DeleteTopUpCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal class DeleteTopUpCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<DeleteTopUpCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteTopUpCommand request, CancellationToken cancellationToken)
    {
        var iResult = await unitOfWork.RepositoryNew<CTopUp>().Entities.Where(w => w.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, false), cancellationToken);
        if (iResult != 1) return await Result<Guid>.FailAsync("Not found the item");
        return await Result<Guid>.SuccessAsync(request.Id, "The item deleted");
    }
}
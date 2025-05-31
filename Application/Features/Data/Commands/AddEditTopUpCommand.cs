using CTopUp = LeUs.Domain.Data.CTopUp;

namespace Leus.Application.Features.Data.Commands;

public class AddEditTopUpCommand : IRequest<Result<Guid>>
{
    public AddEditDataRequest<CTopUpDto> Request { get; set; } = default!;
}

internal class AddEditTopUpCommandHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<AddEditTopUpCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEditTopUpCommand command,
        CancellationToken cancellationToken)
    {
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
            case ActionCommandType.AddApprove:
                var oNewItem = command.Request.Data.Adapt<CTopUp>();
                await unitOfWork.RepositoryNew<CTopUp>().AddAsync(oNewItem);
                //Add Amount to User Balance
                if (command.Request.Action == ActionCommandType.AddApprove && oNewItem.Status == 2)
                {
                    var userId = oNewItem.UserId.ToGuid();
                    var oFind = await unitOfWork.RepositoryAgg<UserBalance>().GetByIdAsync(userId);
                    if (oFind == null)
                    {
                        oFind = new UserBalance()
                        {
                            Id = userId,
                            Amount = (oNewItem.IsDeposit ? 0 : oNewItem.ApproveAmount) ?? 0,
                            DepositAmount = (oNewItem.IsDeposit ? oNewItem.ApproveAmount : 0) ?? 0
                        };
                        await unitOfWork.RepositoryAgg<UserBalance>().AddAsync(oFind);
                    }
                    else
                    {
                        if (oNewItem.IsDeposit)
                        {
                            oFind.DepositAmount += oNewItem.ApproveAmount ?? 0;
                        }
                        else
                        {
                            oFind.Amount += oNewItem.ApproveAmount ?? 0;
                        }

                        await unitOfWork.RepositoryAgg<UserBalance>().UpdateAsync(oFind);
                    }
                }

                await unitOfWork.Commit(cancellationToken);
                return await Result<Guid>.SuccessAsync(oNewItem.Id, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CTopUp>()
                    .GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    if (command.Request.Action != ActionCommandType.Approve && currentItem.Status == 2)
                    {
                        return await Result<Guid>.FailAsync("Not found the item");
                    }

                    command.Request.Data.Adapt(currentItem);
                    await unitOfWork.RepositoryNew<CTopUp>().UpdateAsync(currentItem);
                    //Add Amount to User Balance
                    if (command.Request.Action == ActionCommandType.Approve && currentItem.Status == 2)
                    {
                        var userId = currentItem.UserId.ToGuid();
                        var oFind = await unitOfWork.RepositoryAgg<UserBalance>().GetByIdAsync(userId);
                        if (oFind == null)
                        {
                            oFind = new UserBalance()
                            {
                                Id = userId,
                                Amount = (currentItem.IsDeposit ? 0 : currentItem.ApproveAmount) ?? 0,
                                DepositAmount = (currentItem.IsDeposit ? currentItem.ApproveAmount : 0) ?? 0
                            };
                            await unitOfWork.RepositoryAgg<UserBalance>().AddAsync(oFind);
                        }
                        else
                        {
                            if (currentItem.IsDeposit)
                            {
                                oFind.DepositAmount += currentItem.ApproveAmount ?? 0;
                            }
                            else
                            {
                                oFind.Amount += currentItem.ApproveAmount ?? 0;
                            }
                            await unitOfWork.RepositoryAgg<UserBalance>().UpdateAsync(oFind);
                        }
                    }

                    await unitOfWork.Commit(cancellationToken);
                    return await Result<Guid>.SuccessAsync(currentItem.Id, "The item updated");
                }

                break;
        }

        return await Result<Guid>.FailAsync("Not found the item");
    }
}
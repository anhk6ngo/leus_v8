using Leus.Application.Features.Data.Queries;
using CShipment = LeUs.Domain.Data.CShipment;

namespace Leus.Application.Features.Data.Commands;

public class AddEditShipmentCommand : IRequest<Result<AddEditShipmentResponse>>
{
    public AddEditDataRequest<CShipmentDto> Request { get; set; } = default!;
}

internal class AddEditShipmentCommandHandler(
    IUnitOfWork<Guid, PortalContext> unitOfWork,
    ILeUsService leUsService,
    IMediator mediator)
    : IRequestHandler<AddEditShipmentCommand, Result<AddEditShipmentResponse>>
{
    public async Task<Result<AddEditShipmentResponse>> Handle(AddEditShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var response = new AddEditShipmentResponse();
        var dCost = 0.0;
        if (command.Request.Data!.ApiName == ApiName.FirstMile)
        {
            dCost = await leUsService.GetRate(command.Request.Data);
        }
        command.Request.Data.CalOverSizeFee();
        switch (command.Request.Action)
        {
            case ActionCommandType.Add:
            case ActionCommandType.Confirm:
                var oNewItem = command.Request.Data.Adapt<CShipment>();
                if ($"{oNewItem.Shipper?.Zip}".NotIsNullOrEmpty())
                {
                    oNewItem.ZonePrice =
                        await leUsService.GetZone($"{oNewItem.Shipper?.Zip}", $"{oNewItem.Consignee?.Zip}");
                }

                var priceResponse = await mediator.Send(new GetPriceByCountryQuery()
                {
                    Data = new GetPriceRequest
                    {
                        Country = oNewItem.Consignee?.CountryCode,
                        CustomerId = oNewItem.CustomerId,
                        ServiceId = command.Request.Data?.ServiceId,
                        PackageType = oNewItem.PackageType,
                        UnitType = oNewItem.UnitType,
                        ZonePrice = oNewItem.ZonePrice,
                        TransactionDate = DateTime.Today,
                        VolumeWeight = command.Request.Data?.CalculateVolumeWeight() ?? 0,
                        NetWeight = oNewItem.Weight ?? 0
                    }
                }, cancellationToken);
                if (priceResponse.Price > 0)
                {
                    oNewItem.Price = priceResponse.Price;
                    oNewItem.PriceCode = priceResponse.PriceCode;
                    oNewItem.ChargeWeight = priceResponse.ChargeWeight;

                    if (dCost > oNewItem.Price)
                    {
                        oNewItem.Remote = ((dCost * 1.05 - oNewItem.Price) ?? 0).ToRound(2);
                    }
                    oNewItem.ExcessVolumeFee = priceResponse.ExcessVolumeFee;
                }
                else
                {
                    oNewItem.Price = 0;
                    oNewItem.Remote = 0;
                }

                oNewItem.ServiceCode1 = priceResponse.ServiceCode;
                oNewItem.ApiName1 = priceResponse.ApiName;
                response.ServiceCode = oNewItem.ServiceCode1;
                var sNo = $"{DateTime.Now.ToUtc():MMyy}";
                do
                {
                    var rndNo = $"{sNo}{SharedExtension.Extensions.GenerateRandomNumber(100000, 999999999)}";
                    if (await unitOfWork.RepositoryNew<CShipment>().Entities
                            .AnyAsync(w => w.ReferenceId == rndNo, cancellationToken)) continue;
                    oNewItem.ReferenceId = rndNo;
                    break;
                } while (true);

                await unitOfWork.RepositoryNew<CShipment>().AddAsync(oNewItem);
                await unitOfWork.Commit(cancellationToken);
                oNewItem.Adapt(response);
                return await Result<AddEditShipmentResponse>.SuccessAsync(response, "The item added");
            default:
                var currentItem = await unitOfWork.RepositoryNew<CShipment>()
                    .GetByIdAsync(command.Request.Data!.Id);
                if (currentItem != null)
                {
                    if (currentItem.ShipmentStatus >= 2)
                    {
                        return await Result<AddEditShipmentResponse>.FailAsync(
                            "The shipment have label. You cannot edit this shipment.");
                    }

                    command.Request.Data.ReferenceId = currentItem.ReferenceId;
                    if ($"{currentItem.Shipper?.Zip}" != $"{command.Request.Data.Shipper?.Zip}" ||
                        $"{currentItem.Consignee?.Zip}" != $"{command.Request.Data.Consignee?.Zip}" ||
                        command.Request.Data.ZonePrice == 0)
                    {
                        command.Request.Data.ZonePrice =
                            await leUsService.GetZone($"{command.Request.Data.Shipper?.Zip}",
                                $"{command.Request.Data.Consignee?.Zip}");
                    }

                    command.Request.Data.CreatedOn = currentItem.CreatedOn;
                    command.Request.Data.Adapt(currentItem);
                    priceResponse = await mediator.Send(new GetPriceByCountryQuery()
                    {
                        Data = new GetPriceRequest
                        {
                            Country = currentItem.Consignee?.CountryCode,
                            CustomerId = currentItem.CustomerId,
                            ServiceId = command.Request.Data?.ServiceId,
                            PackageType = currentItem.PackageType,
                            UnitType = currentItem.UnitType,
                            ZonePrice = currentItem.ZonePrice,
                            TransactionDate = DateTime.Today,
                            VolumeWeight = command.Request.Data?.CalculateVolumeWeight() ?? 0,
                            NetWeight = currentItem.Weight ?? 0
                        }
                    }, cancellationToken);
                    currentItem.Price = priceResponse.Price;
                    currentItem.PriceCode = priceResponse.PriceCode;
                    currentItem.ChargeWeight = priceResponse.ChargeWeight;
                    currentItem.ExcessVolumeFee = priceResponse.ExcessVolumeFee;
                    currentItem.ServiceCode1 = priceResponse.ServiceCode;
                    currentItem.ApiName1 = priceResponse.ApiName;
                    if (currentItem is { ChargeWeight: > 0, Price: > 0 })
                    {
                        if (dCost > currentItem.Price)
                        {
                            currentItem.Remote = ((dCost * 1.05 - currentItem.Price) ?? 0).ToRound(2);
                        }
                        else
                        {
                            currentItem.Remote = 0;
                        }
                    }
                    else
                    {
                        currentItem.Remote = 0;
                    }

                    await unitOfWork.RepositoryNew<CShipment>().UpdateAsync(currentItem);
                    await unitOfWork.Commit(cancellationToken);
                    currentItem.Adapt(response);
                    return await Result<AddEditShipmentResponse>.SuccessAsync(response, "The item updated");
                }

                break;
        }

        return await Result<AddEditShipmentResponse>.FailAsync("Not found the item");
    }
}
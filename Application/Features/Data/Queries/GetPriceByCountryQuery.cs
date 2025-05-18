using Leus.Application.Features.Catalog.Queries;

namespace Leus.Application.Features.Data.Queries;

public class GetPriceByCountryQuery : IRequest<GetPriceResponse>
{
    public GetPriceRequest Data { get; set; } = new();
}

internal class GetPriceByCountryQueryHandler(IMediator mediator)
    : IRequestHandler<GetPriceByCountryQuery, GetPriceResponse>
{
    public async Task<GetPriceResponse> Handle(GetPriceByCountryQuery request, CancellationToken cancellationToken)
    {
        var result = new GetPriceResponse();
        var blnCubic = false;
        var services = await mediator.Send(new GetAllServiceQuery(), cancellationToken);
        var oFind = services.FirstOrDefault(w => w.Id == $"{request.Data.ServiceId}".ToGuid());
        if (oFind != null)
        {
            result.ServiceCode = oFind.ServiceCode;
            result.ApiName = oFind.ApiName;
            blnCubic = oFind.UseCubic;
        }

        var prices = await mediator.Send(new GetAllSellingPriceQuery(), cancellationToken);
        prices = prices.Where(w =>
                w.ServiceId == request.Data.ServiceId && w.UnitType == request.Data.UnitType
                                                      && (w.Zones != null && w.Zones!.Any(ws =>
                                                          ws.Country == request.Data.Country))
                                                      && w.FromDate <= request.Data.TransactionDate
                                                      && (w.ToDate >= request.Data.TransactionDate || w.ToDate == null))
            .OrderByDescending(o => o.FromDate).ToList();
        if (request.Data.CustomerId.NotIsNullOrEmpty())
        {
            if (prices.Any(w => w.CustomerId!.Contains($"{request.Data.CustomerId}")))
            {
                prices = prices.Where(w => w.CustomerId!.Contains($"{request.Data.CustomerId}")).ToList();
            }
        }
        else
        {
            prices = prices.Where(w => w.IsPrivate == false).ToList();
        }

        if (prices is { Count: 0 })
        {
            return result;
        }

        var iPackageType = PackageTypeToInt(request.Data.PackageType);
        var price = prices.FirstOrDefault();
        if (price?.Zones is { Count: > 1 } && request.Data.ZonePrice == 0) return result;
        var dCubitWeight = price?.UnitType == 2 ? request.Data.VolumeWeight * 0.0353 : request.Data.VolumeWeight / 1728;
        var dChargeWeight = (request.Data.VolumeWeight / price!.Ratio) * (price.UnitType == 1 ? 16 : 1);
        if (dChargeWeight < request.Data.NetWeight)
        {
            dChargeWeight = request.Data.NetWeight;
        }

        dChargeWeight = dChargeWeight.ToRound(2);
        dCubitWeight = dCubitWeight.ToRound(2);
        if (dCubitWeight < 1 && blnCubic)
        {
            dChargeWeight = request.Data.NetWeight.ToRound(2);
        }
        var blnHasCubit = price.MaxCubic > 0 && dCubitWeight <= price.MaxCubic;

        var oPriceDetails = price.Details?.Where(w =>
                w.Min <= dChargeWeight && (dChargeWeight <= w.Max || w.Max == 0) && w.GoodType == iPackageType &&
                w.ChargeWeightType == 0)
            .ToList();
        var dPriceMin = 0.0;
        if (oPriceDetails is { Count: 0 })
        {
            if (!blnHasCubit)
                return result;
        }
        else
        {
            foreach (var oDetail in oPriceDetails!)
            {
                var aPrice = $"{oDetail.Price}".SplitExt(" ").ToList();
                if (request.Data.ZonePrice > aPrice.Count) return result;
                var dPrice = aPrice[request.Data.ZonePrice - 1].ConvertToDouble();
                if (dPriceMin > dPrice)
                {
                    result.ServiceCode = oDetail.ServiceCode;
                    result.ApiName = services.FirstOrDefault(w => w.ServiceCode == oDetail.ServiceCode)?.ApiName;
                }

                dPriceMin = dPrice;
                if (oDetail.ServiceCode.IsNullOrEmpty() || oDetail.ServiceCode == oFind!.ServiceCode)
                {
                    result.PriceCode = price.PriceCode;
                    result.ChargeWeight = dChargeWeight;
                    switch (oDetail.PriceType)
                    {
                        case 0:
                            result.Price = dPrice;
                            break;
                        default:
                            var remainWeight = dChargeWeight % (oDetail.PriceType == 1 ? .5 : 1);
                            var dTmp = dChargeWeight - remainWeight;
                            if (remainWeight > 0) dTmp += (oDetail.PriceType == 1 ? .5 : 1);
                            result.Price = dTmp * dPrice;
                            break;
                    }
                }
            }
        }

        if (!blnHasCubit) return result;
        //Find min cubit price
        oPriceDetails = price.Details?.Where(w =>
                w.Min <= dCubitWeight && (dCubitWeight <= w.Max || w.Max == 0) && w.GoodType == iPackageType &&
                w.ChargeWeightType == 1)
            .ToList();
        foreach (var oDetail in oPriceDetails!)
        {
            var aPrice = $"{oDetail.Price}".SplitExt(" ").ToList();
            if (request.Data.ZonePrice > aPrice.Count) return result;
            var dPrice = aPrice[request.Data.ZonePrice - 1].ConvertToDouble();
            if (dPriceMin > dPrice)
            {
                result.ServiceCode = oDetail.ServiceCode;
                result.ApiName = services.FirstOrDefault(w => w.ServiceCode == oDetail.ServiceCode)?.ApiName;
                result.PriceCode = $"{result.PriceCode}_1";
            }

            dPriceMin = dPrice;
            if (oDetail.ServiceCode.IsNullOrEmpty() || oDetail.ServiceCode == oFind!.ServiceCode)
            {
                result.PriceCode = price.PriceCode;
                switch (oDetail.PriceType)
                {
                    case 0:
                        result.Price = dPrice;
                        break;
                    default:
                        var remainWeight = dCubitWeight % (oDetail.PriceType == 1 ? .5 : 1);
                        var dTmp = dCubitWeight - remainWeight;
                        if (remainWeight > 0) dTmp += (oDetail.PriceType == 1 ? .5 : 1);
                        result.Price = dTmp * dPrice;
                        break;
                }
            }
        }

        return result;
    }

    private static int PackageTypeToInt(string? packageType)
    {
        return $"{packageType}".ToLower() switch
        {
            "doc" => 1,
            "pak" => 2,
            _ => 0
        };
    }
}
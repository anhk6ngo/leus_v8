using FirstMile;
using LeUs.Application.Dtos.UnitedBridge;

namespace LeUs.Infrastructure.Services;

public static class TransformHelper
{
    public static UBuyRequest ToUnitedBridge(CShipmentDto item)
    {
        var result = new UBuyRequest
        {
            metadata = new UMetaData()
            {
                reference1 = item.ReferenceId,
                reference2 = $"{item.ReferenceId2} {item.ReferenceId3}".Trim(),
            },
            sender = new UAddress()
            {
                name = item.Shipper?.Name,
                address1 = item.Shipper?.AddressLine1,
                address2 = item.Shipper?.AddressLine2,
                city = item.Shipper?.City,
                state = item.Shipper?.State,
                postal_code = item.Shipper?.Zip,
                country = item.Shipper?.CountryCode,
                phone = item.Shipper?.Phone,
            },
            recipient = new UAddress()
            {
                name = item.Consignee?.Name,
                address1 = item.Consignee?.AddressLine1,
                address2 = item.Consignee?.AddressLine2,
                city = item.Consignee?.City,
                state = item.Consignee?.State,
                postal_code = item.Consignee?.Zip,
                country = item.Consignee?.CountryCode,
                phone = item.Consignee?.Phone,
            },
            dimensions = item.Boxes?.Select(s => new UDimensions()
            {
                height = s.Height,
                width = s.Width,
                length = s.Length,
            }).FirstOrDefault(),
            customs_form = item.Customs?.Select(s => new UCustomForm()
            {
                description = s.DestDescription,
                quantity = s.Qty,
                value = s.UnitValue,
                origin_country_code = s.OriginCountryCode,
                hts_code = s.HsCode
            }).ToList(),
            weight = item.Weight ?? 0,
            service = item.ServiceCode
        };
        if (item.ServiceCode == "FEDEX_SMARTPOST" || item.ServiceCode == "FEDEX_GROUND")
        {
            result.package = "YOUR_PACKAGING";
        }

        switch (item.UnitType)
        {
            case 0:
                result.weight_unit = "lbs";
                result.dimensions_unit = "in";
                break;
            case 1:
                result.weight_unit = "oz";
                result.dimensions_unit = "in";
                break;
            default:
                result.weight_unit = "kg";
                result.dimensions_unit = "cm";
                break;
        }

        return result;
    }
    public static DhlLabelRequest ToFirstMile(CShipmentDto item)
    {
        var labelType = item.ServiceCode switch
        {
            CServiceCode.XParcelGround => DomesticLabelType.XParcelGround,
            CServiceCode.XParcelExpedited => DomesticLabelType.XParcelExpedited,
            _ => DomesticLabelType.UPSGround
        };

        var result = new DhlLabelRequest()
        {
            Reference1 = item.ReferenceId,
            Reference2 = $"{item.ReferenceId2} {item.ReferenceId3}".Trim(),
            FromAddress = new Address()
            {
                CompanyName = item.Shipper?.Company,
                Name = item.Shipper?.Name,
                Address1 = item.Shipper?.AddressLine1,
                Address2 = item.Shipper?.AddressLine2,
                City = item.Shipper?.City,
                Region = item.Shipper?.State,
                RegionCode = item.Shipper?.Zip,
                CountryCode = item.Shipper?.CountryCode,
                Country = item.Shipper?.County,
                Email = item.Shipper?.Email,
                PhoneNumber = item.Shipper?.Phone,
                Residential = $"{item.Shipper?.Company}".IsNullOrEmpty(),
            },
            ShipToAddress = new Address()
            {
                CompanyName = item.Consignee?.Company,
                Name = item.Consignee?.Name,
                Address1 = item.Consignee?.AddressLine1,
                Address2 = item.Consignee?.AddressLine2,
                City = item.Consignee?.City,
                Region = item.Consignee?.State,
                RegionCode = item.Consignee?.Zip,
                CountryCode = item.Consignee?.CountryCode,
                Country = item.Consignee?.County,
                Email = item.Consignee?.Email,
                PhoneNumber = item.Consignee?.Phone,
                Residential = $"{item.Consignee?.Company}".IsNullOrEmpty(),
            },
            OrderNumber = new OrderNumberBarCode()
            {
                OrderNumber = item.ReferenceId,
                BarcodeType = DhlBarcodeType.Code128
            },
            IsTest = false,
            LabelImageFormat = ImageFormat.Pdf,
            LabelSize = LabelSize.Label4X6,
            LabelType = labelType,
            PackageDetail = new PackageDetails()
            {
                Packages = item.Boxes!.Select(itemBox => new PackageInfo()
                {
                    PackageDimensions = new PackageDimensions()
                    {
                        HeightInches = (item.UnitType == 2
                            ? itemBox.Height.ConvertCmToInch()
                            : (decimal)itemBox.Height),
                        WidthInches = (item.UnitType == 2
                            ? itemBox.Width.ConvertCmToInch()
                            : (decimal)itemBox.Width),
                        LengthInches = (item.UnitType == 2
                            ? itemBox.Length.ConvertCmToInch()
                            : (decimal)itemBox.Length),
                    },
                    WeightOz = item.UnitType == 2
                        ? itemBox.Weight.ConvertKgsToOz()
                        : (decimal)(itemBox.Weight * (item.UnitType == 0 ? 16 : 1)),
                }).ToArray()
            },
            CustomsData = new CustomsData
            {
                PackageValue = (decimal)(item.Customs?.Sum(s => s.UnitValue) ?? 0),
                PackageDescription = item.Customs?.FirstOrDefault()?.DestDescription,
                CustomsItems = item.Customs?.Select(s => new CustomsItem()
                {
                    Description = s.LocalDescription,
                    ItemDescriptionInExportCountryLanguage = s.DestDescription,
                    HSCode = s.HsCode,
                    ItemQuantity = s.Qty,
                    ItemValue = (decimal)(s.UnitValue ?? 0),
                    ItemWeight = (decimal)(s.UnitWeight ?? 0),
                    ItemCode = s.Sku,
                    CountryOfOrigin = s.OriginCountryCode
                }).ToArray()
            },
            GetRate = true
        };
        switch (item.UnitType)
        {
            case 0:
                result.WeightLbs = (decimal)(item.Weight ?? 0);
                break;
            case 1:
                result.WeightOz = (decimal)(item.Weight ?? 0);
                break;
            default:
                result.WeightKilos = (decimal)(item.Weight ?? 0);
                break;
        }

        return result;
    }
    public static DomesticRateRequest ToFirstMileRate(CShipmentDto item)
    {
        var labelType = item.ServiceCode switch
        {
            CServiceCode.XParcelGround => DomesticLabelType.XParcelGround,
            CServiceCode.XParcelExpedited => DomesticLabelType.XParcelExpedited,
            _ => DomesticLabelType.UPSGround
        };

        var result = new DomesticRateRequest()
        {
            OriginZipCode = $"{item.Shipper?.Zip}",
            DestinationZipCode = $"{item.Consignee?.Zip}",
            ShippingService = labelType,
            PackageDetail = new PackageDetails()
            {
                Packages = item.Boxes!.Select(itemBox => new PackageInfo()
                {
                    PackageDimensions = new PackageDimensions()
                    {
                        HeightInches = (item.UnitType == 2
                            ? itemBox.Height.ConvertCmToInch()
                            : (decimal)itemBox.Height),
                        WidthInches = (item.UnitType == 2
                            ? itemBox.Width.ConvertCmToInch()
                            : (decimal)itemBox.Width),
                        LengthInches = (item.UnitType == 2
                            ? itemBox.Length.ConvertCmToInch()
                            : (decimal)itemBox.Length),
                    },
                    WeightOz = item.UnitType == 2
                        ? itemBox.Weight.ConvertKgsToOz()
                        : (decimal)(itemBox.Weight * (item.UnitType == 0 ? 16 : 1)),
                }).ToArray()
            },
        };
        result.WeightOz = result.PackageDetail.Packages.Sum(s=>s.WeightOz);
        return result;
    }
}
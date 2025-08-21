using LeUs.Application.Dtos.Gps;
using CAddress = LeUs.Application.Dtos.Gps.CAddress;

namespace LeUs.Application.Dtos.Data;

public class CShipmentDto : AggregateRoot<Guid>, IShipment
{
    public bool SyncGetLabel { get; set; } = false;
    public string? ReferenceId { get; set; }
    public string? ReferenceId2 { get; set; }
    public string? ReferenceId3 { get; set; }
    public string? TrackingNo { get; set; }
    public string? EntryPoint { get; set; } = "LAX";
    public string? ServiceCode { get; set; }
    public string? ServiceCode1 { get; set; }
    public int ClearanceType { get; set; } = 0;
    public string? DutyType { get; set; } = "DDU";
    public string? DimensionUnit { get; set; } = "inch";
    public string? FbaCode { get; set; }
    public string? FbaShipmentId { get; set; }
    public string? FbaPoId { get; set; }
    public double? Weight { get; set; } = 0;
    public string? WeightUnit { get; set; } = "lb";
    public string? CustomsCurrency { get; set; } = "USD";
    public string? BoxQty { get; set; } = "1";
    public string? SignatureRequired { get; set; } = "No";
    public string? PackageType { get; set; } = "wpx";
    public CCod? Cod { get; set; }
    public string? BatteryType { get; set; }
    public CPackageCustomerReference? PackageCustomerReferences { get; set; }
    public CAddress? Shipper { get; set; } = new();
    public CAddress? Consignee { get; set; } = new();
    public List<CProduct>? Products { get; set; }
    public List<CCustom>? Customs { get; set; } = [];
    public List<CManifestBox>? Boxes { get; set; } = [];
    public string? PromotionCode { get; set; }
    public double? PromotionAmount { get; set; } = 0;
    public string? CustomerId { get; set; }
    public string? PriceCode { get; set; }
    public double? Price { get; set; } = 0;
    public double? Cost { get; set; } = 0;
    public double? ChargeWeight { get; set; } = 0;
    public int ShipmentStatus { get; set; } = 0;
    public string? ApiName { get; set; }
    public int UnitType { get; set; }
    public string? TrackIds { get; set; }
    public double? Remote { get; set; } = 0;
    public double? CancelFee { get; set; } = 0;
    public double? ExtraLongFee { get; set; } = 0;
    public double? OverLimitFee { get; set; } = 0;
    public double? ExcessVolumeFee { get; set; } = 0;
    public bool IsOverSize { get; set; }
    public DateTime? CreateLabelDate { get; set; }
    public DateTime? CancelLabelDate { get; set; }
    public int ZonePrice { get; set; } = 0;
    public double TotalTime { get; set; } = 0;
    [NotMapped] public DateTime CreatedOn { get; set; }
    [NotMapped] public DateTime ShippedOn { get; set; }
    public string? ServiceId { get; set; }

    public void UnitChanged()
    {
        switch (UnitType)
        {
            case 2:
                WeightUnit = "kg";
                DimensionUnit = "cm";
                break;
            default:
                DimensionUnit = "inch";
                WeightUnit = UnitType == 0 ? "lb" : "oz";
                break;
        }
    }

    public string Dimesion()
    {
        if (Boxes is { Count: 0 }) return "";
        return Boxes!.Select(s => $"{s.Length}x{s.Width}x{s.Height}").Aggregate((a, b) => $"{a} {b}");
    }

    public double TotalPrice() => Price.PlusNumber(Remote).PlusNumber(OverLimitFee).PlusNumber(ExtraLongFee)
        .PlusNumber(ExcessVolumeFee);

    public double CalculateVolumeWeight()
    {
        return Boxes?.Sum(s => s.Height * s.Length * s.Width) ?? 0;
    }

    public void CalOverSizeFee(int iType = 0)
    {
        if (UnitType > 0)
        {
            UnitChanged();
        }

        if (Boxes is { Count: 0 }) return;
        foreach (var box in Boxes!)
        {
            List<double> aDim = [box.Height, box.Width, box.Length];
            aDim = aDim.Order().ToList();
            var dTotal = (aDim[0] + aDim[1]) * 2 + aDim[2];
            if (dTotal > 108)
            {
                IsOverSize = true;
            }

            switch (iType)
            {
                case 1:
                    if (aDim.Any(w => w > 27) || aDim.Count(w => w > 17) > 1)
                    {
                        ExtraLongFee = 6.88;
                    }
                    break;
                default:
                    if (aDim.Any(w => w > 30))
                    {
                        OverLimitFee = 9;
                    }
                    else if (aDim.Any(w => w > 22))
                    {
                        ExtraLongFee = 5;
                    }
                    break;
            }
        }
    }
}
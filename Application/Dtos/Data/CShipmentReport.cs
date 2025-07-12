using LeUs.Application.Dtos.Gps;
using CAddress = LeUs.Application.Dtos.Gps.CAddress;

namespace LeUs.Application.Dtos.Data;

public class CShipmentReport
{
    public string? ReferenceId { get; set; }
    public string? ReferenceId2 { get; set; }
    public string? ReferenceId3 { get; set; }
    public string? TrackingNo { get; set; }
    public string? EntryPoint { get; set; }
    public string? ServiceCode { get; set; }
    public string? DimensionUnit { get; set; }
    public double? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public CAddress? Shipper { get; set; }
    public CAddress? Consignee { get; set; }
    public string? CustomerId { get; set; }
    public string? PriceCode { get; set; }
    public double? Price { get; set; }
    public double? Cost { get; set; }
    public double? ChargeWeight { get; set; }
    public int ShipmentStatus { get; set; }
    public string? ApiName { get; set; }
    public string? TrackIds { get; set; }

    public string? Box
    {
        get
        {
            return this.Boxes?.Select(s => $"{s.Length}x{s.Width}x{s.Height} {s.Weight}").FirstOrDefault();
        }
    }

    public double? Remote { get; set; }
    public double? ExtraLongFee { get; set; } = 0;
    public double? OverLimitFee { get; set; } = 0;
    public double? ExcessVolumeFee { get; set; } = 0;
    public int ZonePrice { get; set; }
    public double TotalTime { get; set; } = 0;
    public List<CManifestBox>? Boxes { get; set; }
    public DateTime? CreateLabelDate { get; set; }
    public DateTime? CancelLabelDate { get; set; }
    public DateTime CreatedOn { get; set; }
}
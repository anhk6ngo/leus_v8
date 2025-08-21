using LeUs.Application.Dtos.Gps;
using CAddress = LeUs.Application.Dtos.Gps.CAddress;

namespace LeUs.Application.Interfaces.Data;

public interface IShipment: IBaseShipment
{
    public bool SyncGetLabel { get; set; }
    public string? DimensionUnit { get; set; }
    public string? WeightUnit { get; set; }
    public string? PromotionCode { get; set; }
    public double? PromotionAmount { get; set; }
    public string? CustomerId { get; set; }
    public double? Cost { get; set; }
    public string? ApiName { get; set; }
    public int ZonePrice { get; set; }
    public double TotalTime { get; set; } 
}

public interface IBaseShipment: IShipmentInput
{
    public string? ReferenceId { get; set; }
    public string? TrackingNo { get; set; }
    public string? PriceCode { get; set; }
    public double? Price { get; set; }
    public double? ChargeWeight { get; set; }
    public int ShipmentStatus { get; set; }
    public int UnitType { get; set; }
    public string? TrackIds { get; set; }
    public double? Remote { get; set; }
    public double? CancelFee { get; set; }
    public double? ExtraLongFee { get; set; }
    public double? OverLimitFee { get; set; }
    public double? ExcessVolumeFee { get; set; }
    public bool IsOverSize { get; set; }
    public DateTime? CreateLabelDate { get; set; }
    public DateTime? CancelLabelDate { get; set; }
}

public interface IShipmentInput
{
    public string? ReferenceId2 { get; set; }
    public string? ReferenceId3 { get; set; }
    public string? EntryPoint { get; set; }
    public string? ServiceCode { get; set; }
    public int ClearanceType { get; set; }
    public string? DutyType { get; set; }
    public string? FbaCode { get; set; }
    public string? FbaShipmentId { get; set; }
    public string? FbaPoId { get; set; }
    public double? Weight { get; set; }
    public string? CustomsCurrency { get; set; }
    public string? BoxQty { get; set; } 
    public string? SignatureRequired { get; set; }
    public string? PackageType { get; set; }
    public CCod? Cod { get; set; }
    public string? BatteryType { get; set; }
    public CPackageCustomerReference? PackageCustomerReferences { get; set; }
    public CAddress? Shipper { get; set; }
    public CAddress? Consignee { get; set; }
    public List<CProduct>? Products { get; set; }
    public List<CCustom>? Customs { get; set; }
    public List<CManifestBox>? Boxes { get; set; }
}
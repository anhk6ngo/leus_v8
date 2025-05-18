namespace LeUs.Application.Dtos.Gps;

public class GpsCreateShipmentRequest
{
    public bool SyncGetLabel { get; set; } = false;
    public string? ReferenceId { get; set; }
    public string? ReferenceId2 { get; set; }
    public string? ReferenceId3 { get; set; }
    public string? TrackingNo { get; set; }
    public string? EntryPoint { get; set; }
    public string? ServiceCode { get; set; }
    public int ClearanceType { get; set; } = 0;
    public string? DutyType { get; set; }
    public string? DimensionUnit { get; set; } = "cm";
    public string? FbaCode { get; set; }
    public string? FbaShipmentId { get; set; }
    public string? FbaPoId { get; set; }
    public double? Weight { get; set; }
    public string? WeightUnit { get; set; } = "kg";
    public string? CustomsCurrency { get; set; } = "USD";
    public string? BoxQty { get; set; } = "1";
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
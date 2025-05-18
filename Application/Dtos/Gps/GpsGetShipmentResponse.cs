using System.ComponentModel.DataAnnotations;

namespace LeUs.Application.Dtos.Gps;

public class GpsGetShipmentResponse
{
    public string? ShipmentId { get; set; }
    public string? ReferenceId { get; set; }
    public string? ReferenceId2 { get; set; }
    public string? ReferenceId3 { get; set; }
    public List<string>? TrackingNo { get; set; }
    public List<string>? CarrierTrackingNos { get; set; }
    public string? EntryPoint { get; set; }
    public string? ServiceCode { get; set; }
    public string? DutyType { get; set; }
    public string? DimensionUnit { get; set; }
    public string? FbaCode { get; set; }
    public string? FbaShipmentId { get; set; }
    public string? FbaPoId { get; set; }
    public double ManifestWeight { get; set; }
    public string? WeightUnit { get; set; }
    public string? CustomsCurrency { get; set; }
    public int ManifestBoxQty { get; set; }
    public int CheckinBoxQty { get; set; }
    public double CheckinWeight { get; set; }
    public double CheckinVolume { get; set; }
    public string? SignatureRequired { get; set; }
    public string? PackageType { get; set; }
    public string? BatteryType { get; set; }
    public CAddress? Shipper { get; set; }
    public CAddress? Consignee { get; set; }
    public List<CProduct>? Products { get; set; }
    public List<CCustom>? Customs { get; set; }
    public List<CManifestBox>? ManifestBoxs { get; set; }
    public List<CBaseBox>? CheckinBoxs { get; set; }
    public List<GpsFee>? Fees { get; set; }
}

public class CProduct
{
    [Description("Quantity of product")]
    public int Qty { get; set; }
    [Description("Sku of product")]
    [MaxLength(50)]
    public string? Sku { get; set; }
}

public class CCustom
{
    [Required]
    [Description("Sku of product")]
    [MaxLength(30)]
    public string? Sku { get; set; }
    [Required]
    [Description("Description in local language")]
    [MaxLength(30)]
    public string? LocalDescription { get; set; }
    [Description("Description in destination language")]
    [MaxLength(30)]
    [Required]
    public string? DestDescription { get; set; }
    [Description("Unit value")]
    [Required]
    public double? UnitValue { get; set; } = 0;
    [Description("Unit weight")]
    [Required]
    public double? UnitWeight { get; set; } = 0;
    [Description("Quantity of product")]
    [Required]
    public int Qty { get; set; } = 1;
    [Description("HS code")]
    [MaxLength(50)]
    public string? HsCode { get; set; }
    [Description("Brand")]
    [MaxLength(50)]
    public string? Brand { get; set; }

    [Description("Origin of country code")]
    [MaxLength(50)]
    public string? OriginCountryCode { get; set; } = "VN";
    [Description("Material in local language")]
    [MaxLength(50)]
    public string? LocalMaterial { get; set; }
    [Description("Material in destination language")]
    [MaxLength(50)]
    public string? DestMaterial { get; set; }
    [Description("Usage in local language")]
    [MaxLength(50)]
    public string? LocalUsage { get; set; }
    [Description("Usage in destination language")]
    [MaxLength(50)]
    public string? DestUsage { get; set; }
    [Description("Model of product")]
    [MaxLength(50)]
    public string? Modal { get; set; }
    [Description("sale channel website url")]
    [MaxLength(50)]
    public string? SaleUrl { get; set; }
    [Description("The image of product url")]
    public string? PicBase64 { get; set; }
}

public class CManifestBox : CBaseBox
{
    [Description("Products in current box")]
    public List<CProduct>? Products { get; set; }
}

public class CBaseBox
{
    [Required]
    [Description("Qty of box")]
    public int BoxQty { get; set; } = 1;
    [Description("Length of box,the dimension unit refer to the param unit type")]
    [Required]
    public double Length { get; set; } = 0;
    [Description("Width of box,the dimension unit refer to the param unit type")]
    [Required]
    public double Width { get; set; } = 0;
    [Description("Height of box,the dimension unit refer to the param unit type")]
    [Required]
    public double Height { get; set; } = 0;
    [Description("Weight of box,the dimension unit refer to the param unit type")]
    [Required]
    public double Weight { get; set; } = 0;
}
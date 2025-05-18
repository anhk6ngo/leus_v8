using System.ComponentModel.DataAnnotations;
using LeUs.Application.Dtos.Gps;
using CAddress = LeUs.Application.Dtos.Gps.CAddress;

namespace LeUs.Application.Dtos.Data;

public class ShipmentDto : AggregateRoot<Guid>, IBaseShipment
{
    [Description("Reference Id，which usually means provider's order id")]
    public string? ReferenceId { get; set; }
    [Description("Reference Id，which usually means customer's order id")]
    [MaxLength(30)]
    public string? ReferenceId2 { get; set; }

    [MaxLength(30)] public string? ReferenceId3 { get; set; }

    [Description("Tracking number,if any")]
    [MaxLength(30)]
    public string? TrackingNo { get; set; }

    [Required]
    [Description("Warehoue code，you can get the code from the service provider")]
    [MaxLength(30)]
    public string? EntryPoint { get; set; } = "LAX";

    [Required]
    [Description("Service code,you can get the code from the service provider")]
    [MaxLength(30)]
    public string? ServiceCode { get; set; }

    [Description("Customs declaration 0 No export tariff rebate 1 Require export tariff rebate")]
    public int ClearanceType { get; set; } = 0;

    [Description("Duty paid type,\nDDP Delivered Duty Paid\nDDU Delivered Duty Unpaid")]
    [MaxLength(3)]
    public string? DutyType { get; set; } = "DDU";

    [Description("FBA warehouse code ,FBA shipment is required")]
    [MaxLength(10)]
    public string? FbaCode { get; set; }

    [Description("FBA Shipment Id")]
    [MaxLength(20)]
    public string? FbaShipmentId { get; set; }

    [MaxLength(20)]
    [Description("FBA Po Id")]
    public string? FbaPoId { get; set; }

    [Required]
    [Description("Weight of shipment")]
    public double? Weight { get; set; } = 0;

    [Description("Currency of customs USD GBP CNY JPY CAD EUR AUD")]
    [MaxLength(3)]
    public string? CustomsCurrency { get; set; } = "USD";

    [Required]
    [Description("Box qty，default is 1 ,pls fill in the actual number of boxs if the shipment has multiple boxs")]
    [MaxLength(30)]
    public string? BoxQty { get; set; } = "1";

    [Description(
        "Whether the face-to-face signature is required.Some service such as DHL FEDEX provide face-to-face signature service. " +
        "Generally, this parameter is not required for small packages\nno Signature is no required\nyes Signature is required\nadult Signature is required adult to sign")]
    public string? SignatureRequired { get; set; }
    [MaxLength(30)]
    [Required]
    [Description(
        "Package type，This parameter is required such as DHL UPS FedEx\ndoc Document\npak Pak\nwpx Package/Box\ndefault is wpx")]
    public string? PackageType { get; set; } = "wpx";

    public CCod? Cod { get; set; }

    [Description("Battery type .Optional values: PI965 PI966 PI967 PI968 PI969 PI970 OTHER")]
    [MaxLength(30)]
    public string? BatteryType { get; set; }

    [Description("Customer References")] public CPackageCustomerReference? PackageCustomerReferences { get; set; }
    [Description("Shipper information")] public CAddress? Shipper { get; set; }

    [Description("Consignee information")]
    [Required]
    public CAddress? Consignee { get; set; }

    public List<CProduct>? Products { get; set; }

    [Description("Customs information")]
    [Required]
    public List<CCustom>? Customs { get; set; }

    [Description("Boxs list")]
    [Required]
    public List<CManifestBox>? Boxes { get; set; }

    [Description("Price Code of shipment")]
    public string? PriceCode { get; set; }

    [Description("Price of shipment")] public double? Price { get; set; } = 0;

    [Description("Charge Weight of shipment")]
    public double? ChargeWeight { get; set; } = 0;

    [Description("This is a shipment status as: draft, genrated label...")]
    public int ShipmentStatus { get; set; } = 0;

    [Required]
    [Description("0: inch/lb, 1: inch/oz, 2: cm/kg")]
    public int UnitType { get; set; } = 0;

    public string? TrackIds { get; set; }
    public double? Remote { get; set; }
    [Description("Cancel Fee")]
    public double? CancelFee { get; set; }
    public DateTime? CreateLabelDate { get; set; }
}
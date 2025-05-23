namespace LeUs.Application.Reponses;

public class AddEditShipmentResponse
{
    [Description("Id. You can use Id for delete shipment")]
    public Guid Id { get; set; }
    [Description("This is a price for shipment")]
    public double? Price { get; set; } = 0;
    [Description("This is a Remote fee.")]
    public double? Remote { get; set; } = 0;
    [Description("ReferenceId. You can use it to generate label, get label, cancel label")]
    public string? ReferenceId { get; set; }
    [Description("This is a price code")]
    public string? PriceCode { get; set; }
    [Description("Charge Weight")]
    public double? ChargeWeight { get; set; } = 0;
    [Description("Extra Long Fee")]
    public double? ExtraLongFee { get; set; }
    [Description("Over Limit Fee")]
    public double? OverLimitFee { get; set; }
    [Description("Excess Volume Fee")]
    public double? ExcessVolumeFee { get; set; }
}
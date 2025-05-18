namespace LeUs.Application.Reponses;

public class AddEditShipmentResponse
{
    public Guid Id { get; set; }
    public double? Price { get; set; } = 0;
    public string? ReferenceId { get; set; }
    public double? Remote { get; set; } = 0;
    public string? PriceCode { get; set; }
    public double? ChargeWeight { get; set; }
}
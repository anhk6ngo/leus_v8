namespace LeUs.Application.Requests;

public class GetPriceRequest
{
    public string? CustomerId { get; set; }
    public string? ServiceId { get; set; }
    public string? Country { get; set; }
    public int UnitType { get; set; }
    public string? PackageType { get; set; }
    public int ZonePrice { get; set; }
    public double VolumeWeight { get; set; }
    public double NetWeight { get; set; }
    public DateTime TransactionDate { get; set; }
}
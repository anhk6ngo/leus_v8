namespace LeUs.Domain.Data;

public class CPrice: AuditableEntityNew<Guid>, ICPrice
{
    public string? PriceCode { get; set; }
    public string? PriceName { get; set; }
    public string? CustomerId { get; set; }
    public string? Currency { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool IsPercent { get; set; }
    public bool IsPrivate { get; set; }
    public string? ServiceId { get; set; }
    public int UnitType { get; set; }
    public int Ratio { get; set; }
    public double MaxCubic { get; set; } = 0;
    public List<CPriceDetail>? Details { get; set; }
    public List<PriceZone>? Zones { get; set; }
}
namespace LeUs.Application.Dtos.Data;

public class CPriceDto: AggregateRoot<Guid>, ICPrice
{
    public string? PriceCode { get; set; }
    public string? PriceName { get; set; }
    public string? CustomerId { get; set; }
    public string? Currency { get; set; }
    public DateTime FromDate { get; set; } = DateTime.Today;
    public DateTime? ToDate { get; set; }
    public bool IsPercent { get; set; } = false;
    public bool IsPrivate { get; set; } = false;
    public string? ServiceId { get; set; }
    public int UnitType { get; set; } = 0;
    public int Ratio { get; set; } = 139;
    public double MaxCubic { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public List<CPriceDetail>? Details { get; set; }
    public List<PriceZone>? Zones { get; set; }
}
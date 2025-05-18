namespace LeUs.Application.Interfaces.Data;

public interface ICPrice
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
    public double MaxCubic { get; set; }
    public List<CPriceDetail>? Details { get; set; }
    public List<PriceZone>? Zones { get; set; }
}

public interface ICPriceDetail
{
    public int GoodType { get; set; }
    public int PriceType { get; set; }
   
    public string? Price { get; set; }
    public int Min { get; set; }
    public double Max { get; set; }
    public string? ServiceCode { get; set; }
    public int ChargeWeightType { get; set; }
}

public class PriceZone
{
    public string? Country { get; set; }
    public string? Zone { get; set; }
}
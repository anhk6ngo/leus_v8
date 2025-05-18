namespace LeUs.Domain.Data;

public class CPriceDetail: AggregateRoot<Guid>, ICPriceDetail
{
    public Guid CPriceId { get; set; }
    public int GoodType { get; set; }
    public int PriceType { get; set; }
    public int Min { get; set; }
    public double Max { get; set; }
    public string? Price { get; set; }
    public string? ServiceCode { get; set; }
    public int ChargeWeightType { get; set; } = 0;
}
namespace LeUs.Application.Reponses;

public class GetPriceResponse
{
    public double Price { get; set; } = 0;
    public string? PriceCode { get; set; } = "";
    public double ChargeWeight { get; set; } = 0;
    public string? ServiceCode { get; set; }
    public string? ApiName { get; set; }
}
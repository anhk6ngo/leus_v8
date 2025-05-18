namespace LeUs.Application.Reponses;

public class SumShipmentResponse
{
    public int ActiveNo { get; set; }
    public int GenerateLabelNo { get; set; }
    public int DeActiveNo { get; set; }
    public double Amount { get; set; }
    public string? ServiceCode { get; set; }
    public DateTime TransDate { get; set; }
}
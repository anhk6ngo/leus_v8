namespace LeUs.Application.Reponses;

public class SumShipmentResponse
{
    public int ActiveNo { get; set; }
    public int GenerateLabelNo { get; set; }
    public int CancelLabelNo { get; set; }
    public int DeActiveNo { get; set; }
    public double Amount { get; set; }
    public double Cost { get; set; }
    public string? CustomerId { get; set; }
    public double TotalAmount => Amount + Remote + ExtraLong + OverLimit + ExcessVolume;
    public double Profit => Amount + Remote + ExtraLong + OverLimit + ExcessVolume - Cost;
    public double Remote { get; set; }
    public double ExtraLong { get; set; }
    public double OverLimit { get; set; }
    public double ExcessVolume { get; set; }
    public int Status { get; set; } = 0;
    public string? ServiceCode { get; set; }
    public string? State { get; set; }
    public DateTime TransDate { get; set; }
}
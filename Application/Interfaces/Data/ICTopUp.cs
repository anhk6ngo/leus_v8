namespace LeUs.Application.Interfaces.Data;

public interface ICTopUp
{
    public string? UserId { get; set; }
    public double? RequestAmount { get; set; }
    public DateTime? RequestDate { get; set; }
    public double? ApproveAmount { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? Note { get; set; }
    public string? AccNote { get; set; }
    public string? TransactionId { get; set; }
    public string? Currency { get; set; }
    public int GateWay { get; set; }
    public bool IsDeposit { get; set; }
    public int Status { get; set; }
}
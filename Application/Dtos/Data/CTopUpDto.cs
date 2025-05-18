namespace LeUs.Application.Dtos.Data;

public class CTopUpDto: AggregateRoot<Guid>, ICTopUp
{
    public string? UserId { get; set; }
    public double? RequestAmount { get; set; } = 0;
    public DateTime? RequestDate { get; set; } = DateTime.UtcNow;
    public double? ApproveAmount { get; set; } = 0;
    public DateTime? ApproveDate { get; set; }
    public string? Note { get; set; }
    public string? AccNote { get; set; }
    public string? TransactionId { get; set; }
    public string? Currency { get; set; } = "usd";
    public int GateWay { get; set; } = 0;
    public bool IsDeposit { get; set; } = false;
    public int Status { get; set; } = 0; }
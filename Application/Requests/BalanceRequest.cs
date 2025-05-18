namespace LeUs.Application.Requests;

public class BalanceRequest
{
    public string? UserId { get; set; }
    public double Amount { get; set; } = 0;
    public ActionCommandType Action { get; set; } = ActionCommandType.Delete;
}
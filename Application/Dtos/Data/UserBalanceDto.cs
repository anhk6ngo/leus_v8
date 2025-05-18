namespace LeUs.Application.Dtos.Data;

public class UserBalanceDto: AggregateRoot<Guid>, IUserBalance
{
    public double Amount { get; set; }
    public double DepositAmount { get; set; }
}
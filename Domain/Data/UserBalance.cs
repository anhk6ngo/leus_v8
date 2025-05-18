namespace LeUs.Domain.Data;

public class UserBalance : AggregateRoot<Guid>, IUserBalance
{
    public double Amount { get; set; } = 0;
    public double DepositAmount { get; set; }
}
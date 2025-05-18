namespace LeUs.Domain.Catalog;

public class CCustomer : AuditableEntityNew<Guid>, ICCustomer
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? TaxNo { get; set; }
    public string? BankAccount { get; set; }
    public string? BankName { get; set; }
    public bool IsPublic { get; set; }
    public DateTime? SignContract { get; set; }
}
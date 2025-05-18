namespace LeUs.Domain.Catalog;

public class CCountry : AuditableEntityNew<Guid>, ICCountry
{
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public int Continent { get; set; }
}
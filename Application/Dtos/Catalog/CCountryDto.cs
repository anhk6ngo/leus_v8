namespace LeUs.Application.Dtos.Catalog;

public class CCountryDto : AggregateRoot<Guid>, ICCountry
{
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public int Continent { get; set; } = 0;
}
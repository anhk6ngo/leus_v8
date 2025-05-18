namespace LeUs.Application.Interfaces.Catalog;

public interface ICCountry
{
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public int Continent { get; set; }
}
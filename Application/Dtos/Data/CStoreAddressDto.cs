namespace LeUs.Application.Dtos.Data;

public class CStoreAddressDto: AggregateRoot<Guid>, IBaseAddress
{
    public string? Name { get; set; }
    public string? Company { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? County { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? CountryCode { get; set; } = "US";
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? PhoneNumberExt { get; set; }
    public string? Email { get; set; }
    public string? IdNo { get; set; }
    public string? TaxNo { get; set; }
    public string? TaxNoType { get; set; }
    public string? TaxNoIssuerCountryCode { get; set; }
    public string? ServiceCode { get; set; }
    public string? EntryPoint { get; set; } = "LAX";
    public string? CustomerId { get; set; }
}
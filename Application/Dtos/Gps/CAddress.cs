using System.ComponentModel.DataAnnotations;

namespace LeUs.Application.Dtos.Gps;

public class CAddress : IBaseAddress
{
    [Required] [MaxLength(50)] public string? Name { get; set; }
    [MaxLength(50)] public string? Company { get; set; }
    [MaxLength(50)] public string? RoomNo { get; set; }
    [Required] [MaxLength(50)] public string? AddressLine1 { get; set; }
    [MaxLength(50)] public string? AddressLine2 { get; set; }
    [MaxLength(50)] public string? County { get; set; }
    [Required] [MaxLength(50)] public string? City { get; set; }
    [MaxLength(50)] public string? State { get; set; }
    [MaxLength(2)] [Required] public string? CountryCode { get; set; }

    [Required]
    [MaxLength(50)]
    [Description("Postal code")]
    public string? Zip { get; set; }

    [Required] [MaxLength(50)] public string? Phone { get; set; }
    [MaxLength(50)] public string? PhoneNumberExt { get; set; }
    [MaxLength(50)] public string? Email { get; set; }

    [MaxLength(50)]
    [Description("ID number")]
    public string? IdNo { get; set; }

    [MaxLength(50)]
    [Description("Tax No")]
    public string? TaxNo { get; set; }

    [MaxLength(50)]
    [Description(
        "Tax number type .Optional values: VAT DAN DTF TAN DUN EIN EOR FED FTZ SSN STA CNP SDT IOSS NO-IOSS OTHER")]
    public string? TaxNoType { get; set; }

    [MaxLength(50)]
    [Description("Tax no issuer country code")]
    public string? TaxNoIssuerCountryCode { get; set; }
}
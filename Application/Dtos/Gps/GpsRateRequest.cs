namespace LeUs.Application.Dtos.Gps;

public class GpsRateRequest
{
    public string? EntryPoint { get; set; }
    public string? ServiceCode { get; set; }
    public string? PackageType { get; set; }
    public string? CountryCode { get; set; }
    public string? FbaCode { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Street2 { get; set; }
    public string? Zip { get; set; }
    public string? SensitiveAttributes { get; set; }
    public string? WeightUnit { get; set; }
    public string? DimensionUnit { get; set; }
    public double? Volume { get; set; }
    public double? Weight { get; set; }
    public double? Length { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public List<GpsBoxList>? BoxList { get; set; }
}

public class GpsBoxList
{
    public int? BoxQty { get; set; }
    public double? Weight { get; set; }
    public double? Length { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
}
namespace LeUs.Application.Dtos.Gps;

public class GpsRateResponse
{
    public string? FormulaDesc { get; set; }
    public double Fees { get; set; }
    public string? ZoneNo { get; set; }
    public string? ServiceCode { get; set; }
    public int ChargeableWeight { get; set; }
    public string? Formula { get; set; }
    public string? Currency { get; set; }
    public string? Remark { get; set; }
    public string? ServiceName { get; set; }
    public List<GpsRateDetail>? Detail { get; set; }
}

public class GpsRateDetail
{
    public double Fees { get; set; }
    public string? Formula { get; set; }
    public string? FormulaDesc { get; set; }
    public string? TypeCode { get; set; }
}
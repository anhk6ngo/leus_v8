namespace LeUs.Application.Requests;

public class GetReportRequest
{
    public int TypeReport { get; set; } = 0;
    public bool Export { get; set; } = false;
    public int TypeData { get; set; } = 0;
    public int Status { get; set; } = 0;
    public int TypeExport { get; set; } = 0;
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public List<string> Users { get; set; } = [];
    public bool IsTimeOut { get; set; } = false;
}
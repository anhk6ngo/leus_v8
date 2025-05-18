namespace LeUs.Application.Requests;

public class GetShipmentRequest
{
    [Description("Date Range with format: MM/dd/yy - MM/dd/yy. Exp: 02/14/25 - 02/21/25")]
    public string? DateRange { get; set; }
    [Description("ReferenceId. You can input one or more reference IDs. Exp: 0001 0002")]
    public string? ReferenceId { get; set; }
    public int Status { get; set; } = 0;
    [Description("Page Number")]
    public int PageNumber { get; set; } = 1;
    [Description("Page Size")]
    public int PageSize { get; set; } = 10;
}
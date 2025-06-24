namespace LeUs.Application.Requests;

public class GetShipmentRequest
{
    [Description("Date Range with format: MM/dd/yy - MM/dd/yy. Exp: 02/14/25 - 02/21/25")]
    public string? DateRange { get; set; }
    [Description("ReferenceId. You can input one or more reference IDs. Exp: 0001 0002.")]
    public string? ReferenceId { get; set; }
    [Description("ReferenceId2. You can input one or more reference2 IDs. The system only search by ReferenceId2 if ReferenceId is null. Exp: 0001 0002. ")]
    public string? ReferenceId2 { get; set; }
    [Description("Tracking Id. You can input one or more tracking IDs. The system only search by ReferenceId2 if ReferenceId is null.Exp: 920019037317014332 9200190373170143329.")]
    public string? TrackingId { get; set; }
    public int Status { get; set; } = 0;
    [Description("Page Number")]
    public int PageNumber { get; set; } = 1;
    [Description("Page Size")]
    public int PageSize { get; set; } = 10;
    [Description("Search these shipments that have total time generate label greaten than 20 second")]
    public bool IsTimeOut { get; set; } = false;
}
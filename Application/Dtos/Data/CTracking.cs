namespace LeUs.Application.Dtos.Data;

public class CTrackingResponse
{
    public string? TrackingNumber { get; set; }
    public string? ReferenceId { get; set; }
    public List<CTrackingEvent> Events { get; set; } = [];
}

public class CTrackingEvent
{
    public string? Date { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
}
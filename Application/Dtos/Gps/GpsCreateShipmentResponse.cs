namespace LeUs.Application.Dtos.Gps;

public class GpsCreateShipmentResponse
{
    public string? ReferenceId{get;set;}
    public string? ShipmentId { get; set; }
    public string? TrackingNo { get; set; }
    public List<string>? TrackingNos { get; set; }
    public string? LabelBase64 { get; set; }
    public List<LabelDetail>? LabelBase64s { get; set; }
    public List<GpsFee>? Fees { get; set; }
}
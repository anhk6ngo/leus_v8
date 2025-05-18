namespace LeUs.Application.Dtos.Gps;

public class GpsTrackResponse
{
    public string? OriginCountryCode { get; set; }
    public string? DestCountryCode { get; set; }
    public List<GpsTrace>? Traces { get; set; }
}

public class GpsTrace
{
    [JsonPropertyName("event_code")] public string? Code { get; set; }
    [JsonPropertyName("event_location")] public string? Location { get; set; }

    [JsonPropertyName("event_description")]
    public string? Description { get; set; }

    [JsonPropertyName("event_time")] public string? Time { get; set; }
    [JsonPropertyName("event_status")] public string? Status { get; set; }
}
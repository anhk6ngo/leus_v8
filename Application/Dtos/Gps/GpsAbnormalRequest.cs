namespace LeUs.Application.Dtos.Gps;

public class GpsAbnormalRequest: GpsTrackRequest
{
    public string? ExceptionCode { get; set; }
    public string? FollowDescription { get; set; }
}


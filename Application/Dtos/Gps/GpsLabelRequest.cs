namespace LeUs.Application.Dtos.Gps;

public class GpsLabelRequest: GpsTrackRequest
{
    public bool EncodeByBase64 { get; set; } = true;
}


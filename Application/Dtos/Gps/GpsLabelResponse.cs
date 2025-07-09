namespace LeUs.Application.Dtos.Gps;

public class GpsLabelResponse
{
    public string? LabelBase64 { get; set; }
    public string? TrackingNo { get; set; }
    public string? LabelUrl { get; set; }
    public List<string>? TrackingNos { get; set; }
    public List<LabelDetail>? LabelBase64S { get; set; }
}

public class LabelDetail
{
    public string? Label { get; set; }
    public string? TrackignNo { get; set; }
}

public class LabelDetailExt : LabelDetail
{
    public string? Ref2 { get; set; }
}

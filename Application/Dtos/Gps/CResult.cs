namespace LeUs.Application.Dtos.Gps;

public class CResult<T> where T : class
{
    public string? RequestId{get;set;}
    public string? TrackingId { get; set; }
    public bool? Success{get;set;}
    public int ErrorCode{get;set;}
    public string? ErrorMessage{get;set;}
    public long Timestamp{get;set;}
    public T? Data{get;set;}
}
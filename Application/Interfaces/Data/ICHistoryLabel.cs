namespace LeUs.Application.Interfaces.Data;

public interface ICHistoryLabel
{
    public string? Response { get; set; }
    public string? ReferenceId { get; set; }
    public string? Request { get; set; }
    public DateTime CratedOn { get; set; }
}
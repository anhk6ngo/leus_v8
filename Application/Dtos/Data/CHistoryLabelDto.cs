namespace LeUs.Application.Dtos.Data;

public class CHistoryLabelDto: ICHistoryLabel
{
    public string? Response { get; set; }
    public string? ReferenceId { get; set; }
    public string? Request { get; set; }
    public string? ApiName { get; set; }
    public bool Status { get; set; }
    public DateTime CratedOn { get; set; } = DateTime.UtcNow;
}
namespace LeUs.Application.Dtos.Data;

public class CHistoryLabelDto: AggregateRoot<int>, ICHistoryLabel
{
    public string? Response { get; set; }
    public string? ReferenceId { get; set; }
    public string? Request { get; set; }
    public DateTime CratedOn { get; set; } = DateTime.UtcNow;
}
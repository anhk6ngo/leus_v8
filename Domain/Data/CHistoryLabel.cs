namespace LeUs.Domain.Data;

public class CHistoryLabel: AggregateRoot<Guid>, ICHistoryLabel
{
    public string? Response { get; set; }
    public string? ReferenceId { get; set; }
    public string? Request { get; set; }
    public string? ApiName { get; set; }
    public bool Status { get; set; } = false;
    public DateTime CratedOn { get; set; } = DateTime.UtcNow;
}
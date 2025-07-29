using LeUs.Application.Dtos.Gps;

namespace LeUs.Application.Extensions;

public record ShipmentLabel(
    Guid Id,
    string? ApiName,
    string? ApiName1,
    string? ShipmentId,
    string? ServiceCode,
    List<LabelDetail>? Labels,
    string? TrackIds);

public record LogResult(string? ApiName, DateTime CreatedOn, string? ReferenceId, string? Response);

public static class QueryShipmentExtension
{
    public static IQueryable<ShipmentLabel> GetShipmentLabels(this IQueryable<CShipment> query, string userId,
        List<string> refIds) =>
        query.Where(w => refIds.Contains(w.ReferenceId!) && w.CreatedBy == userId && w.ShipmentStatus == 2)
            .ProjectToType<ShipmentLabel>();

    public static IQueryable<CShipment>
        ApplyLabelDate(this IQueryable<CShipment> query, DateTime dFrom, DateTime dTo, int iStatus, string? userId) =>
        query.Where(w =>
            (iStatus == 2 ? w.CreateLabelDate : w.CancelLabelDate) >= dFrom &&
            (iStatus == 2 ? w.CreateLabelDate : w.CancelLabelDate) <= dTo && w.ShipmentStatus == iStatus &&
            (userId.IsNullOrEmpty() || w.CreatedBy== userId));

    public static IQueryable<CShipment> ApplyStatus(this IQueryable<CShipment> query, int status) =>
        query.Where(w => w.ShipmentStatus == status);
    
    public static IQueryable<CTopUpDto>
        ApplyTopUp(this IQueryable<CTopUp> query, DateTime dFrom, DateTime dTo, int iStatus, string? userId="") =>
        query.Where(w => w.IsActive && w.CreatedOn >= dFrom && w.CreatedOn <= dTo &&
            (userId.IsNullOrEmpty() || w.UserId== userId) &&
            (iStatus == 0 || w.Status == iStatus)).ProjectToType<CTopUpDto>();
    public static IQueryable<LogResult> GetLogShipment(this IQueryable<CHistoryLabel> query, DateTime dateFrom,
        DateTime dateTo, bool status) =>
        query.Where(w => w.CratedOn >= dateFrom && w.CratedOn <= dateTo && w.Status == status)
            .Select(s => new LogResult(
                s.ApiName,
                s.CratedOn,
                s.ReferenceId,
                s.Response
            ));
}
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

public static class QueryShipmentExtension
{
    public static IQueryable<ShipmentLabel> GetShipmentLabels(this IQueryable<CShipment> query, string userId,
        List<string> refIds) =>
        query.Where(w => refIds.Contains(w.ReferenceId!) && w.CreatedBy == userId && w.ShipmentStatus == 2)
            .ProjectToType<ShipmentLabel>();
}
using CTopUp = LeUs.Domain.Data.CTopUp;

namespace Leus.Application.Features.Data.Queries;

public class GetAllTopUpByUserQuery : IRequest<List<CTopUpDto>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int Status { get; set; } = 0;
    public int TypeData { get; set; } = 0;
}

internal class GetAllTopUpQueryHandler(IUnitOfWork<Guid, PortalContext> context)
    : IRequestHandler<GetAllTopUpByUserQuery, List<CTopUpDto>>
{
    public async Task<List<CTopUpDto>> Handle(GetAllTopUpByUserQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        if (request.TypeData == 1)
        {
            return await context.RepositoryNew<CTopUp>().Entities
                .Where(w=> w.IsActive && w.CreatedOn <= oDateRange.dTo && w.Status == 2)
                .AsNoTracking()
                .ProjectToType<CTopUpDto>()
                .OrderByDescending(o=>o.RequestDate)
                .Take(100)
                .ToListAsync(cancellationToken);
        }
        var result = await context.RepositoryNew<CTopUp>().Entities
            .ApplyTopUp(oDateRange.dFrom, oDateRange.dTo, request.Status, request.UserId)
            .ToListAsync(cancellationToken);
        return result;
    }
}
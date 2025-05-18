using CTopUp = LeUs.Domain.Data.CTopUp;

namespace Leus.Application.Features.Data.Queries;

public class GetAllTopUpByUserQuery : IRequest<List<CTopUpDto>>
{
    public string? DateRange { get; set; }
    public string? UserId { get; set; }
    public int Status { get; set; } = 0;
    public int TypeData { get; set; } = 0;
}

internal class GetAllTopUpQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetAllTopUpByUserQuery, List<CTopUpDto>>
{
    public async Task<List<CTopUpDto>> Handle(GetAllTopUpByUserQuery request,
        CancellationToken cancellationToken)
    {
        var oDateRange = $"{request.DateRange}".ConvertRangeDate(lastday: true, isUtc: true);
        var oFilter = PredicateBuilder.True<CTopUp>();
        if (request.TypeData == 1)
        {
            oFilter = oFilter.And(w => w.IsActive  && w.RequestDate <= oDateRange.dTo && w.Status == 2);
            return await unitOfWork.RepositoryNew<CTopUp>().Entities
                .Where(oFilter)
                .ProjectToType<CTopUpDto>()
                .AsNoTracking()
                .OrderByDescending(o=>o.RequestDate)
                .Take(100)
                .ToListAsync(cancellationToken);
        }
        oFilter = oFilter.And(w => w.IsActive && w.RequestDate >= oDateRange.dFrom
                                              && w.RequestDate <= oDateRange.dTo);
        if (request.UserId.NotIsNullOrEmpty())
        {
            oFilter = oFilter.And(w => w.UserId == request.UserId);
        }

        if (request.Status > 0)
        {
            oFilter = oFilter.And(w => w.Status == request.Status);
        }
        var result = await unitOfWork.RepositoryNew<CTopUp>().Entities
            .Where(oFilter)
            .ProjectToType<CTopUpDto>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return result;
    }
}
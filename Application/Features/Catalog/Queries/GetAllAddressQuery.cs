namespace Leus.Application.Features.Catalog.Queries;

public class GetAllAddressQuery : IRequest<List<CAddressDto>>
{
    public string? UserId { get; set; }
}

internal class GetAllAddressQueryHandler(IUnitOfWork<Guid, PortalContext> unitOfWork)
    : IRequestHandler<GetAllAddressQuery, List<CAddressDto>>
{
    public async Task<List<CAddressDto>> Handle(GetAllAddressQuery request, CancellationToken cancellationToken)
    {
        return await
            unitOfWork.RepositoryNew<CAddress>()
                .Entities.Where(w => w.IsActive == true && w.CreatedBy == request.UserId)
                .ProjectToType<CAddressDto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }
}
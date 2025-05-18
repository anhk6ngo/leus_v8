namespace Leus.Application.Features.Catalog.Queries;

public class GetAllCustomerByEmailQuery : IRequest<string?>
{
    public string? Email { get; set; }
}

internal class GetAllCustomerByEmailQueryHandler(IMediator mediator)
    : IRequestHandler<GetAllCustomerByEmailQuery, string?>
{
    public async Task<string?> Handle(GetAllCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customers = await mediator.Send(new GetAllCustomerQuery(), cancellationToken);
        var oFind = customers.FirstOrDefault(w => $"{w.Email}".Contains($"{request.Email}"));
        return oFind != null ? $"{oFind.Id}" : string.Empty;
    }
}
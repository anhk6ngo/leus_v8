namespace Leus.Application.Validators;

public class GetShipmentByUserValidator : AbstractValidator<GetShipmentRequest>
{
    public GetShipmentByUserValidator()
    {
        RuleFor(rq => rq.DateRange).NotEmpty().WithMessage("The Date Range field is required")
            .When(x => RequireDate(x.ReferenceId));
    }

    private static bool RequireDate(string? shipmentId)
    {
        return $"{shipmentId}".IsNullOrEmpty();
    }
}
namespace Leus.Application.Validators;

public class CPriceDtoValidator : AbstractValidator<CPriceDto>
{
    public CPriceDtoValidator()
    {
        RuleFor(rq => rq.ServiceId).NotEmpty().WithMessage("The Service field is required");
        RuleFor(rq => rq.PriceCode).NotEmpty().WithMessage("The Price Code field is required");
        RuleFor(rq => rq.PriceName).NotEmpty().WithMessage("The Price Name field is required");
    }
}
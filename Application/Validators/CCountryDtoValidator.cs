namespace Leus.Application.Validators;

public class CCountryDtoValidator : AbstractValidator<CCountryDto>
{
    public CCountryDtoValidator()
    {
        RuleFor(rq => rq.CountryCode).NotEmpty().WithMessage("The Code field is required").MaximumLength(2);
        RuleFor(rq => rq.CountryName).NotEmpty().WithMessage("The Name field is required");
    }
}
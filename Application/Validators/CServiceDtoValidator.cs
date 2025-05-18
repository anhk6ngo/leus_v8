namespace Leus.Application.Validators;

public class CServiceDtoValidator : AbstractValidator<CServiceDto>
{
    public CServiceDtoValidator()
    {
        RuleFor(rq => rq.ServiceCode).NotEmpty().WithMessage("The Code field is required").MaximumLength(36);
        RuleFor(rq => rq.ServiceName).NotEmpty().WithMessage("The Name field is required").MaximumLength(255);
        RuleFor(rq => rq.ApiName).MaximumLength(255);
    }
}
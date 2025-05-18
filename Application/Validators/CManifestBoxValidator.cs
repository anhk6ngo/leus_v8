using LeUs.Application.Dtos.Gps;

namespace Leus.Application.Validators;

public class CManifestBoxValidator : AbstractValidator<CManifestBox>
{
    public CManifestBoxValidator()
    {
        RuleFor(rq => rq.BoxQty).GreaterThan(0).WithMessage("The Quantity field must be greater than 0");
        RuleFor(rq => rq.Weight).GreaterThan(0).WithMessage("The Weight field must be greater than 0");
        RuleFor(rq => rq.Height).GreaterThan(0).WithMessage("The Height field must be greater than 0");
        RuleFor(rq => rq.Length).GreaterThan(0).WithMessage("The Length field must be greater than 0");
        RuleFor(rq => rq.Width).GreaterThan(0).WithMessage("The Width field must be greater than 0");
    }
}
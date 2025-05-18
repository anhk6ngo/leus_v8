using LeUs.Application.Dtos.Gps;

namespace Leus.Application.Validators;

public class CCustomValidator : AbstractValidator<CCustom>
{
    public CCustomValidator()
    {
        RuleFor(rq => rq.DestDescription).NotEmpty().WithMessage("The Description Dest field is required");
        RuleFor(rq => rq.LocalDescription).NotEmpty().WithMessage("The Description Local field is required");
        RuleFor(rq => rq.Qty).GreaterThan(0).WithMessage("The Quantity field is required");
        RuleFor(rq => rq.UnitValue).GreaterThan(0).WithMessage("The Unit Value field is required");
        RuleFor(rq => rq.UnitWeight).GreaterThan(0).WithMessage("The Unit Weight field is required");
    }
}
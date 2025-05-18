namespace Leus.Application.Validators;

public class CCustomerDtoValidator : AbstractValidator<CCustomerDto>
{
    public CCustomerDtoValidator()
    {
        RuleFor(rq => rq.Code).NotEmpty().WithMessage("The Code field is required").MaximumLength(50);
        RuleFor(rq => rq.Name).NotEmpty().WithMessage("The Name field is required").MaximumLength(255);
        RuleFor(rq => rq.Email).EmailAddress().MaximumLength(255);
        RuleFor(rq => rq.Address).MaximumLength(500);
        RuleFor(rq => rq.TaxNo).MaximumLength(50);
        RuleFor(rq => rq.ContactPerson).MaximumLength(255);
        RuleFor(rq => rq.Phone).MaximumLength(50);
        RuleFor(rq => rq.BankAccount).MaximumLength(50);
        RuleFor(rq => rq.BankName).MaximumLength(255);
    }
}
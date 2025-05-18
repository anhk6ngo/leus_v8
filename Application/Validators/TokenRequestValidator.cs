namespace Leus.Application.Validators;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(rq => rq.Email).NotEmpty().WithMessage("The Email field is required").EmailAddress();
        RuleFor(rq => rq.Password).NotEmpty().WithMessage("The Password field is required");
    }
}
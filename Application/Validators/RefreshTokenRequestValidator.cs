namespace Leus.Application.Validators;

public class RefreshTokenRequestValidator: AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(rq => rq.Token).NotEmpty().WithMessage("The Token field is required");
        RuleFor(rq => rq.RefreshToken).NotEmpty().WithMessage("The Refresh Token field is required");
    }
}
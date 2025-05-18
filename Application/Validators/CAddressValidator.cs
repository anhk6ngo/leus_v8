using LeUs.Application.Dtos.Gps;
using CAddress = LeUs.Application.Dtos.Gps.CAddress;

namespace Leus.Application.Validators;

public class ConsigneeAddressValidator : GpsAddressBaseValidator
{
    public ConsigneeAddressValidator()
    {
        RuleFor(rq => rq.CountryCode).NotEmpty().WithMessage("The Country field is required");
        RuleFor(rq => rq.Name).NotEmpty().WithMessage("The Name field is required");
        RuleFor(rq => rq.AddressLine1).NotEmpty().WithMessage("The Address field is required");
        RuleFor(rq => rq.Phone).NotEmpty().WithMessage("The Phone field is required");
        RuleFor(rq => rq.Zip).NotEmpty().WithMessage("The PostCode field is required");
        RuleFor(rq => rq.City).NotEmpty().WithMessage("The City Name field is required");
    }
}

public class GpsAddressBaseValidator : AbstractValidator<CAddress>
{
    public GpsAddressBaseValidator()
    {
        RuleFor(rq => rq.Company).MaximumLength(50);
        RuleFor(rq => rq.Name).MaximumLength(50);
        RuleFor(rq => rq.RoomNo).MaximumLength(50);
        RuleFor(rq => rq.AddressLine1).MaximumLength(50);
        RuleFor(rq => rq.AddressLine2).MaximumLength(50);
        RuleFor(rq => rq.County).MaximumLength(50);
        RuleFor(rq => rq.City).MaximumLength(50);
        RuleFor(rq => rq.State).MaximumLength(50);
        RuleFor(rq => rq.CountryCode).MaximumLength(2);
        RuleFor(rq => rq.Zip).MaximumLength(50);
        RuleFor(rq => rq.Phone).MaximumLength(50);
        RuleFor(rq => rq.PhoneNumberExt).MaximumLength(50);
        RuleFor(rq => rq.Email).MaximumLength(50);
        RuleFor(rq => rq.IdNo).MaximumLength(50);
        RuleFor(rq => rq.TaxNo).MaximumLength(50);
        RuleFor(rq => rq.TaxNoType).MaximumLength(50);
        RuleFor(rq => rq.TaxNoIssuerCountryCode).MaximumLength(2);
    }
}
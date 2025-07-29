namespace Leus.Application.Validators;

public class CShipmentDtoValidator : AbstractValidator<CShipmentDto>
{
    public CShipmentDtoValidator()
    {
        RuleFor(rq => rq.ServiceCode).NotEmpty().WithMessage("The Service field is required");
        RuleFor(rq => rq.Consignee).SetValidator(new ConsigneeAddressValidator()!);
        RuleForEach(rq => rq.Boxes).SetValidator(new CManifestBoxValidator());
        RuleFor(rq => rq.Shipper).SetValidator(new GpsAddressBaseValidator()!);
        RuleSet("Gps", () =>
        {
            RuleFor(rq => rq.EntryPoint).NotEmpty().WithMessage("The Entry Point field is required");
            RuleFor(rq => rq.PackageType).NotEmpty().WithMessage("The Package Type field is required");
            RuleFor(rq => rq.CustomsCurrency).NotEmpty().WithMessage("The Customs Currency field is required")
                .MaximumLength(3);
            RuleFor(rq => rq.Customs)
                .Must(x => x!.Count > 0).WithMessage("The Customs field is required")
                .ForEach(k =>
                {
                    k.SetValidator(new CCustomValidator());
                });
        });
    }
}
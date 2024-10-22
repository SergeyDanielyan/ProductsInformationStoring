using FluentValidation;

namespace OzonHW2.Validators;

public class UpdatePriceRequestValidator : AbstractValidator<UpdatePriceRequest>
{
    public UpdatePriceRequestValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.NewPrice).NotNull().GreaterThan(0);
    }
}
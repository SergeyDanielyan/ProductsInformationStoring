using FluentValidation;

namespace OzonHW2.Validators;

public class GoodsIdValidator : AbstractValidator<GoodsId>
{
    public GoodsIdValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}
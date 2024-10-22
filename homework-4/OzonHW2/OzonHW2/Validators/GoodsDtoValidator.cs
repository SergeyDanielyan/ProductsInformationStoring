using FluentValidation;

namespace OzonHW2.Validators;

public class GoodsDtoValidator : AbstractValidator<GoodsDto>
{
    public GoodsDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Price).NotNull().GreaterThan(0);
        RuleFor(x => x.Weight).NotNull().GreaterThan(0);
        RuleFor(x => x.MyProductType).NotNull();
        RuleFor(x => x.CreationDate).NotNull();
        RuleFor(x => x.WarehouseNumber).NotNull();
    }
}
using FluentValidation;

namespace OzonHW2.Validators;

public class FilterRequestValidator : AbstractValidator<FilterRequest>
{
    public FilterRequestValidator()
    {
        RuleFor(x => x.MyProductType).NotNull();
        RuleFor(x => x.CreationDate).NotNull();
        RuleFor(x => x.WarehouseNumber).NotNull().GreaterThan(0);
        RuleFor(x => x.PageLength).NotNull().GreaterThan(0);
        RuleFor(x => x.PageNumber).NotNull().GreaterThan(0);
    }
}
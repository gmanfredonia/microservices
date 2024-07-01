using Admin.Domain.Contracts.Products;
using Admin.Domain.Contracts.Security;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Admin.API.Extensions
{
    public class DTOLoginValidator : AbstractValidator<DTOLogin>
    {
        public DTOLoginValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage(localizer["messageRequired"]);
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(localizer["messageRequired"]);
        }
    }

    public class DTOProductValidator : AbstractValidator<DTOProduct>
    {
        public DTOProductValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.Id).NotNull().WithMessage(localizer["messageRequired"])
                              .NotEmpty().WithMessage(localizer["messageRequired"]);
            RuleFor(x => x.Name).NotNull().WithMessage(localizer["messageRequired"])
                                .NotEmpty().WithMessage(localizer["messageRequired"])
                                .MaximumLength(50).WithMessage(localizer["messageMaxLength"]);
            RuleFor(x => x.UseType).Matches("^(I|U)?$").WithMessage(localizer["messageUseType"]);
        }
    }
}

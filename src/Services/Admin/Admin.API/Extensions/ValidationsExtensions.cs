
using Admin.Domain.Contracts;
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
}

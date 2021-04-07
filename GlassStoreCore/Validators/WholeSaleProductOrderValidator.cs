using FluentValidation;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;

namespace GlassStoreCore.Validators
{
    public class WholeSaleProductOrderValidator : AbstractValidator<WholeSaleSellingOrdersDto>
    {
        public WholeSaleProductOrderValidator()
        {
            RuleFor(order => order.OrderDate)
                .NotNull()
                .NotEmpty();
            RuleFor(order => order.UserId)
                .NotNull()
                .NotEmpty();
            RuleFor(order => order.WholeSaleSellingOrderDetails)
                .NotNull();
        }
    }
}

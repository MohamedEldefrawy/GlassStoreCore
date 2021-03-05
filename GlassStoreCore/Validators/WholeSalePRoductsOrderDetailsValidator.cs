using FluentValidation;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.Validators
{
    public class WholeSalePRoductsOrderDetailsValidator : AbstractValidator<WholeSaleProductsOrderDetailsDto>
    {
        public WholeSalePRoductsOrderDetailsValidator()
        {

            RuleFor(orderDetail => orderDetail.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
            RuleFor(orderDetail => orderDetail.Quantity)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            RuleFor(orderDetail => orderDetail.WholeSaleProductId)
                .NotNull()
                .NotEmpty();
            RuleFor(orderDetail => orderDetail.WholeSaleSellingOrderId)
                .NotNull()
                .NotEmpty();

        }
    }
}

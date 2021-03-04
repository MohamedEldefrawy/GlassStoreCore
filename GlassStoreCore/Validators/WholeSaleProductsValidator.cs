using FluentValidation;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.Validators
{
    public class WholeSaleProductsValidator : AbstractValidator<WholeSaleProductsDto>
    {
        public WholeSaleProductsValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .NotNull()
                .Length(2, 20);

            RuleFor(product => product.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThan(-1);

            RuleFor(product => product.UnitsInStock)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(product => product.SerialNumber)
                .NotNull()
                .NotEmpty();
        }
    }
}

using FluentValidation;
using ProductManagement.Domain;

namespace ProductManagement.Api
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(product => product.ProductCode).NotEmpty();
            RuleFor(product => product.Category).NotEmpty();
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.Price).GreaterThanOrEqualTo(0);
            RuleFor(product => product.Quantity).GreaterThanOrEqualTo(0);
        }
    }
}

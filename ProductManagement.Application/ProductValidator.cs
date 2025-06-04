using FluentValidation;

namespace ProductManagement.Application
{
    public class ProductValidator : AbstractValidator<SaveProduct>
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

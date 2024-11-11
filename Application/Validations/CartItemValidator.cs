using FluentValidation;
using Infrastracture.Models;


namespace Application.Validations
{
    public  class CartItemValidator: AbstractValidator<CartItem>
    {
        public CartItemValidator() 
        {
            RuleFor(item => item.DesignId).GreaterThan(0).LessThan(100);
            RuleFor(item => item.ProductId).GreaterThan(0).LessThan(5);
            RuleFor(item => item.Quantity).GreaterThan(0).LessThan(10);
        }
    }
}

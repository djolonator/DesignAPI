﻿using FluentValidation;
using Infrastracture.Models;


namespace Application.Validations
{
    public  class CartItemValidator: AbstractValidator<CartItemModel>
    {
        public CartItemValidator() 
        {
            RuleFor(item => item.DesignId).GreaterThan(0);
            RuleFor(item => item.ProductId).GreaterThan(0);
            RuleFor(item => item.Quantity).GreaterThan(0).LessThan(10);
        }
    }
}

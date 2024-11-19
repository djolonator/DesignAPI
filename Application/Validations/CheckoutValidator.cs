

namespace Application.Validations
{
    using FluentValidation;
    using Infrastracture.Models;

    public class CheckoutValidator : AbstractValidator<CheckoutRequest>
    {
        public CheckoutValidator()
        {
            RuleFor(checkout => checkout.Recipient).SetValidator(new RecipientValidator()!);

            RuleFor(checkout => checkout.CartItems)
              .NotEmpty().WithMessage("CartItems cannot be empty.")
              .Must(cartItems => cartItems.Any()).WithMessage("CartItems cannot be empty.");

            RuleForEach(checkout => checkout.CartItems).SetValidator(new CartItemValidator());
        }
    }
}



namespace Application.Validations
{
    using FluentValidation;
    using Infrastracture.Models;

    public class CheckoutValidator : AbstractValidator<CheckoutRequest>
    {
        public CheckoutValidator()
        {
            //RuleFor(checkout => checkout.Recipient).NotNull();
            RuleFor(checkout => checkout.Recipient).SetValidator(new RecipientValidator()!);
            RuleForEach(checkout => checkout.CartItems).SetValidator(new CartItemValidator());
        }
    }
}

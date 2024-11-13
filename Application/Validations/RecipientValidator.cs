using FluentValidation;
using Infrastracture.Models;


namespace Application.Validations
{
    public class RecipientValidator : AbstractValidator<Recipient>
    {
        public RecipientValidator()
        {
            RuleFor(recipient => recipient.Email).NotNull();
            RuleFor(recipient => recipient.City).NotNull();
            RuleFor(recipient => recipient.Address).NotNull();
            RuleFor(recipient => recipient.Country).NotNull();
            RuleFor(recipient => recipient.Name).NotNull();
            RuleFor(recipient => recipient.LastName).NotNull();
            RuleFor(recipient => recipient.Phone).NotNull();
        }
    }
}

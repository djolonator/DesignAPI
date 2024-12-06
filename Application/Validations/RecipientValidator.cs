using FluentValidation;
using Infrastracture.Models;


namespace Application.Validations
{
    public class RecipientValidator : AbstractValidator<RecipientModel>
    {
        public RecipientValidator()
        {
            RuleFor(recipient => recipient.Email).NotEmpty();
            RuleFor(recipient => recipient.City).NotEmpty();
            RuleFor(recipient => recipient.Address).NotEmpty();
            RuleFor(recipient => recipient.Country).NotEmpty();
            RuleFor(recipient => recipient.FirstName).NotEmpty();
            RuleFor(recipient => recipient.LastName).NotEmpty();
            RuleFor(recipient => recipient.Phone).NotEmpty();
        }
    }
}

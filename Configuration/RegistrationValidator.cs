using FluentValidation;
using FluentValidation.Validators;

using Recommend_Movie_System.Models.Request;

namespace Recommend_Movie_System.Configuration
{
    public class RegistrationValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationValidator()
        {
            RuleFor(u => u.firstName)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("First name cannot be empty and maximum length of 30 letters.");

            RuleFor(u => u.lastName)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Last name cannot be empty and maximum length of 30 letters.");


            RuleFor(u => u.email)
                .NotEmpty()
                .EmailAddress(EmailValidationMode.Net4xRegex)
                .WithMessage("Email not match the rules!");

            RuleFor(u => u.password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(30)
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$")
                .WithMessage("Password not match the rules!");
        }
    }
}
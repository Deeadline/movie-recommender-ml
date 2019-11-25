using FluentValidation;
using Recommend_Movie_System.Models.Request;

namespace Recommend_Movie_System.Configuration
{
    public class LoginModelValidator : AbstractValidator<LoginRequest>
    {
        public LoginModelValidator()
        {
            RuleFor(l => l.email)
                .NotEmpty()
                .WithMessage("email is required");

            RuleFor(l => l.password)
                .NotEmpty()
                .WithMessage("password is required");
        }
    }
}
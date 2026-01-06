using FluentValidation;
using Users.Microservice.Application.Commands;

namespace FGC.Application.Users.Commands.UpdateUser
{
    
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("Name is required.")
              .MaximumLength(100)
              .WithMessage("Name must be less than or equal to 100 characters.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MaximumLength(100)
                .WithMessage("Name must be less than or equal to 100 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100)
                .WithMessage("Password must be less than or equal to 100 characters.")
                .Matches(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least one letter, one number, and one special character.")
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

           
        }
    }
}

using FluentValidation;
using WebApi.Application.UserOperations.Commands.CreateToken;

namespace WebApi.UserOperations.Commands.CreateToken
{
    public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenCommandValidator()
        {
            RuleFor(command => command.Model.Email).NotEmpty();
            RuleFor(command => command.Model.Password).NotEmpty().MinimumLength(4);
        }
    }
}
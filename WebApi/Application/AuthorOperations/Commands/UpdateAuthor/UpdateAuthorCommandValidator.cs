using System;
using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.AuthorId).GreaterThan(0);
            RuleFor(command => command.Model.Name).MinimumLength(2).When(x => x.Model.Name.Trim() != string.Empty);
            RuleFor(command => command.Model.Surname).MinimumLength(2).When(x => x.Model.Surname.Trim() != string.Empty);
            RuleFor(command => command.Model.DateOfBirth).NotEmpty().LessThan(new DateTime((DateTime.Now.Year - 18),1,1)).WithMessage("18 yaşından büyük olmalı.");
        }
    }
}
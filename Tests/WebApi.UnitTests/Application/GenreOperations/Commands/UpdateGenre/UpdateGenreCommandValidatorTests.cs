using FluentAssertions;
using WebApi.Application.GenreOperations.Command.UpdateGenre;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests
    {
        [Theory]
        [InlineData(0,"")]
        [InlineData(0,"History")]
        [InlineData(0,"His")]
        [InlineData(1,"His")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int genreId, string name)
        {
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            UpdateGenreModel model = new UpdateGenreModel() { Name = name };

            command.GenreId = genreId;
            command.Model = model;

            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);

        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            UpdateGenreModel model = new UpdateGenreModel() { Name = "Test_WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError" };
            
            command.GenreId = 1;
            command.Model = model;

            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Equals(0);
        }
    }
}
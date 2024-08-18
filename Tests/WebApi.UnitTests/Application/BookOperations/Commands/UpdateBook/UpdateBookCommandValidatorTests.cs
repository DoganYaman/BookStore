using FluentAssertions;
using WebApi.Application.BookOperations.Commands.UptadeBook;
using Xunit;
using static WebApi.Application.BookOperations.Commands.UptadeBook.UpdateBookCommand;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandValidatorTests
    {
        [Theory]
        [InlineData(0,"",0,0)]
        [InlineData(0,"",0,1)]
        [InlineData(0,"",1,0)]
        [InlineData(0,"",1,1)]
        [InlineData(0,"Lor",0,0)]
        [InlineData(0,"Lor",0,1)]
        [InlineData(0,"Lor",1,0)]
        [InlineData(0,"Lor",1,1)]
        [InlineData(0," ",0,0)]
        [InlineData(0," ",0,1)]
        [InlineData(0," ",1,0)]
        [InlineData(0," ",1,1)]
        [InlineData(0,"Lord Of The Rings",0,0)]
        [InlineData(0,"Lord Of The Rings",0,1)]
        [InlineData(0,"Lord Of The Rings",1,0)]
        [InlineData(1,"",0,0)]
        [InlineData(1,"",0,1)]
        [InlineData(1,"",1,0)]
        [InlineData(1,"",1,1)]
        [InlineData(1,"Lor",0,0)]
        [InlineData(1,"Lor",0,1)]
        [InlineData(1,"Lor",1,0)]
        [InlineData(1,"Lor",1,1)]
        [InlineData(1," ",0,0)]
        [InlineData(1," ",0,1)]
        [InlineData(1," ",1,0)]
        [InlineData(1," ",1,1)]
        [InlineData(1,"Lord Of The Rings",0,0)]
        [InlineData(1,"Lord Of The Rings",0,1)]
        [InlineData(1,"Lord Of The Rings",1,0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int bookId, string title, int genreId, int authorId)
        {
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = bookId;
            command.Model = new UpdateBookModel()
            {
                Title = title,
                GenreId = genreId,
                AuthorId = authorId
            };
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            //arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 1;
            command.Model = new UpdateBookModel()
            {
                Title = "Hobbit",
                GenreId = 1,
                AuthorId = 1
            };

            //act
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().Equals(0);
        }
    }
}
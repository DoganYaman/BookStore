using System;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UptadeBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;
using static WebApi.Application.BookOperations.Commands.UptadeBook.UpdateBookCommand;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public UpdateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNonExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            // var book = _context.Books.OrderByDescending(x => x.Id).FirstOrDefault();
            var book = new Book() { Title = "Test_WhenValidInputsAreGiven_Book_ShouldBeUpdated"};
            _context.Books.Add(book);
            _context.SaveChanges();

            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = (book.Id + 1);

            //act & assert
            FluentActions.Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Güncellenecek Kitap Bulunamadı!");

        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            // var book = _context.Books.FirstOrDefault();
            var book = new Book() { Title = "Test_WhenValidInputsAreGiven_Book_ShouldBeUpdated"};
            _context.Books.Add(book);
            _context.SaveChanges();
            
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = book.Id;
            command.Model = new UpdateBookModel()
            {
                Title = "Test",
                GenreId = 1,
                AuthorId = 2
            };

            FluentActions.Invoking(() => command.Handle()).Invoke();
            book.Title.Should().Be(command.Model.Title);
            book.GenreId.Should().Be(command.Model.GenreId);
            book.AuthorId.Should().Be(command.Model.AuthorId);

        }
    }
}
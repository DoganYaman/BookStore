using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.DeleteBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public DeleteBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNonExistBookId_InvalidOperationException_ShouldBeReturn()
        {
            var book = new Book() { Title = "Test_WhenNonExistBookId_InvalidOperationException_ShouldBeReturn"};
            _context.Books.Add(book);
            _context.SaveChanges();

            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = book.Id + 1;

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Silinecek Kitap Bulunamadı!");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_Book_ShouldBeDeleted()
        {
            var book = new Book() { Title = "Test_WhenExistBookIdIsGiven_Book_ShouldBeDeleted"};
            _context.Books.Add(book);
            _context.SaveChanges();
            
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = book.Id;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            book = _context.Books.SingleOrDefault(x => x.Id == command.BookId);
            book.Should().BeNull();
        }
    }
}
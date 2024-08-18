using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.DeleteAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public DeleteAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNonExistAuthorId_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author() { Name = "TestDelete_Name_WhenNonExistAuthorId_InvalidOperationException_ShouldBeReturn", Surname = "TestDelete_Surname_WhenNonExistAuthorId_InvalidOperationException_ShouldBeReturn"};
            _context.Authors.Add(author);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = (author.Id + 1);

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Silinecek Yazar Bulunamadı.");
        }

        [Fact]
        public void WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted()
        {
            var author = new Author() { Name = "TestDelete_Name_WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted", Surname = "TestDelete_Surname_WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted"};
            _context.Authors.Add(author);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = author.Id;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            author = _context.Authors.SingleOrDefault(x => x.Id == command.AuthorId);
            author.Should().BeNull();
        }
    }
}
using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public UpdateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNonExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author() { Name = "TestUpdate_Name_WhenNonExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn", Surname = "TestUpdate_Surname_WhenNonExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn" , DateOfBirth = DateTime.Now.AddYears(-19)};
            _context.Authors.Add(author);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = (author.Id + 1);

            FluentActions.Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar Bulunamadı.");

        }

        [Fact]
        public void WhenAlreadyExistNameAndSurnameAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            var firstAuthor = new Author() { Name = "TestUpdate_Name_First_WhenAlreadyExistNameAndSurnameAreGiven_InvalidOperationException_ShouldBeReturn", Surname ="TestUpdate_Surname_First_WhenAlreadyExistNameAndSurnameAreGiven_InvalidOperationException_ShouldBeReturn", DateOfBirth = DateTime.Now.AddYears(-19) };
            _context.Authors.Add(firstAuthor);
            _context.SaveChanges();

            var secondAuthor = new Author() { Name = "TestUpdate_Name_Second_WhenAlreadyExistNameAndSurnameAreGiven_InvalidOperationException_ShouldBeReturn", Surname ="TestUpdate_Surname_Second_WhenAlreadyExistNameAndSurnameAreGiven_InvalidOperationException_ShouldBeReturn", DateOfBirth = DateTime.Now.AddYears(-19) };
            _context.Authors.Add(secondAuthor);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            UpdateAuthorModel model = new UpdateAuthorModel();
            model.Name = firstAuthor.Name;
            model.Surname = firstAuthor.Surname;

            command.AuthorId = secondAuthor.Id;
            command.Model = model;

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aynı ad ve soyad'a ait bir yazar zaten mevcut");

        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeUpdated()
        {
            var author = new Author() { Name = "TestUpdate_Name_WhenValidInputsAreGiven_Author_ShouldBeUpdated", Surname ="TestUpdate_Surname_WhenValidInputsAreGiven_Author_ShouldBeUpdated", DateOfBirth = DateTime.Now.AddYears(-19) };
            _context.Authors.Add(author);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            var model = new UpdateAuthorModel() { Name = "TestUpdate_Model_Name_WhenValidInputsAreGiven_Author_ShouldBeUpdated", Surname ="TestUpdate_Model_Surname_WhenValidInputsAreGiven_Author_ShouldBeUpdated", DateOfBirth = DateTime.Now.AddYears(-19)};
            command.AuthorId = author.Id;
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            author = _context.Authors.SingleOrDefault(x => x.Id == author.Id);
            author.Should().NotBeNull();
            author.Name.Should().Be(model.Name);
            author.Surname.Should().Be(model.Surname);
        }
    }
}
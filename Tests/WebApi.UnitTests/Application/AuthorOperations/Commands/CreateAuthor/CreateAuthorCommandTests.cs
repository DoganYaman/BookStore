using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.CreateAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper; 
        }

        [Fact]
        public void WhenAlreadyExistFirstAndLastNameAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author() { Name = "TestCreate_FirstName_WhenAlreadyExistFirstAndLastNameAreGiven_InvalidOperationException_ShouldBeReturn", Surname = "TestCreate_LastName_WhenAlreadyExistFirstAndLastNameAreGiven_InvalidOperationException_ShouldBeReturn"};
            _context.Authors.Add(author);
            _context.SaveChanges();

            CreateAuthorCommand command = new CreateAuthorCommand(_context,_mapper);
            command.Model = new CreateAuthorModel() { Name=author.Name, Surname=author.Surname };

            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aynı ad ve soyad'a ait bir yazar zaten mevcut");
        }

        [Fact]
        public void WhenValidInputAreGiven_Author_ShouldBeCreated()
        {
            CreateAuthorCommand command = new CreateAuthorCommand(_context,_mapper);
            CreateAuthorModel model = new CreateAuthorModel() {  Name = "TestCreate_FirstName_WhenValidInputAreGiven_Author_ShouldBeCreated", Surname = "TestCreate_LastName_WhenValidInputAreGiven_Author_ShouldBeCreated", DateOfBirth=DateTime.Now.AddYears(-19)};
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var author = _context.Authors.SingleOrDefault(x => x.Name == model.Name && x.Surname == model.Surname);

            author.Should().NotBeNull();
            author.DateOfBirth.Should().Be(model.DateOfBirth);
        }
    }
}
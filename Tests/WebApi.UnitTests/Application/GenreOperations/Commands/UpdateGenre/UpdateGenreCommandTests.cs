using System;
using System.Linq;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Command.UpdateGenre;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;

        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNonExistGenreIdGiven_InvalidOperationException_ShouldBeReturn()
        {
            var genre = new Genre() { Name = "TestUpdate_WhenNonExistGenreIdGiven_InvalidOperationException_ShouldBeReturn" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = (genre.Id + 1);

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Türü Bulunamadı!");
        }

        [Fact]
        public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var firstGenre = new Genre() { Name = "TestUpdate_WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn_1" };
            _context.Genres.Add(firstGenre);
            _context.SaveChanges();

            var secondGenre = new Genre() { Name = "TestUpdate_WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn_2" };
            _context.Genres.Add(secondGenre);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            UpdateGenreModel model = new UpdateGenreModel();
            model.Name = firstGenre.Name;

            command.GenreId = secondGenre.Id;
            command.Model = model;

            FluentActions.Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aynı isimli bir kitap türü zaten mevcut");

            
        }
        
        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeUpdated()
        {
            var genre = new Genre() { Name = "TestUpdate_WhenValidInputsAreGiven_Genre_ShouldBeUpdated" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            var model = new UpdateGenreModel() { Name = "TestUpdate_Updated_WhenValidInputsAreGiven_Genre_ShouldBeUpdated" };
            command.GenreId = genre.Id;
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            genre = _context.Genres.SingleOrDefault(x => x.Id == genre.Id);
            genre.Should().NotBeNull();
            genre.Name.Should().Be(model.Name);
            genre.IsActive.Should().Be(model.IsActive);
        }
    }
}
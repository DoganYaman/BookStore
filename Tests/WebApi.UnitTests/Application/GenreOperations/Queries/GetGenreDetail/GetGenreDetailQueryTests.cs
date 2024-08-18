using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNonExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var genre = new Genre() { Name = "TestGetDetail_WhenNonExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn" };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = (genre.Id + 1);

            FluentActions.Invoking(() => query.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Türü Bulunamadı!");
        }

        [Fact]
        public void WhenValidInputIsGiven_Genre_ShouldBeReturn()
        {
            var genre = new Genre() { Name = "TestGetDetail_WhenValidInputIsGiven_Genre_ShouldBeReturn" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = genre.Id;

            FluentActions.Invoking(() => query.Handle()).Invoke();

            genre = _context.Genres.SingleOrDefault(x => x.Id == genre.Id);

            genre.Should().NotBeNull();
        }
    }
}
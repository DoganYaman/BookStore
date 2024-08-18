using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public GetBookDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNonExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // var book = _context.Books.OrderByDescending(x => x.Id).FirstOrDefault();
            var book = new Book() { Title = "Test_WhenNonExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn"};
            _context.Books.Add(book);
            _context.SaveChanges();

            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = (book.Id + 1);

            FluentActions.Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Bulunamadı!");
        }

        [Fact]
        public void WhenExistBookIdisGiven_Book_ShouldBeReturn()
        {
            var book = new Book() { Title = "Test_WhenExistBookIdisGiven_Book_ShouldBeReturn"};
            _context.Books.Add(book);
            _context.SaveChanges();

            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = book.Id;

            FluentActions.Invoking(() => query.Handle());
            
            book = _context.Books.SingleOrDefault( x => x.Id == query.BookId);
            book.Should().NotBeNull();
        }
    }
}
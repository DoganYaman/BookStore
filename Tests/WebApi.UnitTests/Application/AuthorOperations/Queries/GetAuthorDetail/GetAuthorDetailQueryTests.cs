using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNonExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var author = new Author() { Name = "TestGetDetail_Name_WhenNonExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn", Surname = "TestGetDetail_Surname_WhenNonExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn" };
            _context.Authors.Add(author);
            _context.SaveChanges();
            
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = (author.Id + 2);

            FluentActions.Invoking(() => query.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar Bulunamadı!");
        }

        [Fact]
        public void WhenValidInputIsGiven_Author_ShouldBeReturn()
        {
            var author = new Author() { Name = "TestGetDetail_Name_WhenValidInputIsGiven_Author_ShouldBeReturn", Surname = "TestGetDetail_Surname_WhenValidInputIsGiven_Author_ShouldBeReturn" };
            _context.Authors.Add(author);
            _context.SaveChanges();

            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = author.Id;

            FluentActions.Invoking(() => query.Handle()).Invoke();

            author = _context.Authors.SingleOrDefault(x => x.Id == author.Id);

            author.Should().NotBeNull();
        }
    }
}
using System;
using System.Linq;
using WebApi.DBOperations;

namespace WebApi.BookOperations.UptadeBook
{
    public class UpdateBookCommand
    {
        private readonly BookStoreDbContext _dbContext;
        public int BookId { get; set; }
        public UpdateBookModel Model { get; set; }

        public UpdateBookCommand(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);

            if(book is null)
                throw new InvalidOperationException("Güncellenecek Kitap Bulunamadı!");
            
            book.Title = Model.Title != default ? Model.Title : book.Title;
            book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId;

            _dbContext.SaveChanges();

        }

        public class UpdateBookModel
        { 
            public string Title { get; set; }     
            public int GenreId { get; set; } 
        }
    }
}
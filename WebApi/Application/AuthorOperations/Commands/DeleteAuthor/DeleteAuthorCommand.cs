using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand
    {
        public int AuthorId { get; set; }
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.Include(x => x.Books).Where(x => x.Id == AuthorId).SingleOrDefault();

            if(author is null)
                throw new InvalidOperationException("Silinecek Yazar Bulunamadı.");

            if(author.Books.Any())
                throw new InvalidOperationException("Yazarın kitabı yayında olduğu için yazar silinemedi. Öncelikle yazarın yayında olan kitapları silinmeli.");
            
            _context.Authors.Remove(author);
            _context.SaveChanges();

        }
    }
}
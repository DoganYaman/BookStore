using System;
using System.Linq;
using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand
    {
        public int AuthorId { get; set; }
        public UpdateAuthorModel Model { get; set; }
        private readonly BookStoreDbContext _context;

        public UpdateAuthorCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);

            if(author is null)
                throw new InvalidOperationException("Yazar Bulunamadı.");
            
            if(_context.Authors.Any(x => x.Name.ToLower() == Model.Name.ToLower() && x.Surname == Model.Surname && x.Id != AuthorId))
                throw new InvalidOperationException("Aynı ad ve soyad'a ait bir yazar zaten mevcut");
            
            author.Name = string.IsNullOrEmpty(Model.Name.Trim()) ? author.Name : Model.Name;
            author.Surname = string.IsNullOrEmpty(Model.Surname.Trim()) ? author.Surname : Model.Surname;
            author.DateOfBirth = Model.DateOfBirth == default ? author.DateOfBirth : Model.DateOfBirth;

            _context.SaveChanges();
        }
    }

    public class UpdateAuthorModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
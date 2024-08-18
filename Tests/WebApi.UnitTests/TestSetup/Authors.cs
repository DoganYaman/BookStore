using System;
using WebApi.DBOperations;
using WebApi.Entities;

namespace TestSetup
{
    public static class Authors
    {
        public static void AddAuthors(this BookStoreDbContext context)
        {
            context.Authors.AddRange(
                    new Author{ Name = "Eric", Surname = "Rice", DateOfBirth = new DateTime(1978,09,22)},
                    new Author{ Name = "Charlotte", Surname = "Gilman", DateOfBirth = new DateTime(1860,07,03)},
                    new Author{ Name = "Frank", Surname = "Herbert", DateOfBirth = new DateTime(1920,10,08)},
                    new Author{ Name = "Yaşar", Surname = "Kemal", DateOfBirth = new DateTime(1923,10,06)},
                    new Author{ Name = "Orhan", Surname = "Pamuk", DateOfBirth = new DateTime(1952,06,07)}
                );
        }
    }
}
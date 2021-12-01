using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Repository.Repositories
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Autor> Autors { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

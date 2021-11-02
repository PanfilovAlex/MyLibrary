using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiMyLib.Models
{
    public class BookDbContext:DbContext
    {
        public BookDbContext (DbContextOptions<BookDbContext> options) : base(options){ }
       public DbSet<Book> Books { get; set; }
    }
}

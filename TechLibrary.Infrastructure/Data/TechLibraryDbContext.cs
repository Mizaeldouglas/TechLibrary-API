using Microsoft.EntityFrameworkCore;
using TechLibrary.Domain.Entities;
using TechLibrary.Persistence.Abstractions;

namespace TechLibrary.Infrastructure.Data;

public class TechLibraryDbContext : DbContext, ITechLibraryDbContext
{
    public TechLibraryDbContext(DbContextOptions<TechLibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    
}
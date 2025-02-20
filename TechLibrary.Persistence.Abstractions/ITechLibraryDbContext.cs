using Microsoft.EntityFrameworkCore;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Persistence.Abstractions;

public interface ITechLibraryDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Book> Books { get; set; }

    int SaveChanges();
    // Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
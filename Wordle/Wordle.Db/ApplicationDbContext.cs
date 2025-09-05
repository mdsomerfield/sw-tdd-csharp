using Microsoft.EntityFrameworkCore;

namespace Wordle.Db;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

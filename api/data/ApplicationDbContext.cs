using api.models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  {
  }

  public DbSet<Stock> Stocks { get; set; }
  public DbSet<Comment> Comments { get; set; }
}
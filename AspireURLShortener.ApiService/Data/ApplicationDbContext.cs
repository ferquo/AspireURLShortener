using Microsoft.EntityFrameworkCore;

namespace AspireURLShortener.ApiService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Entities.ShortenedUrl> ShortenedUrls { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Entities.ShortenedUrl>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LongUrl).IsRequired();
            entity.Property(e => e.ShortUrl).IsRequired();
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.CreatedOnUtc).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}

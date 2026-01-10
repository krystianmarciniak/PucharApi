using Microsoft.EntityFrameworkCore;
using PucharApi.Domain;

namespace PucharApi.Infrastructure;

public class PucharDbContext : DbContext
{
  public PucharDbContext(DbContextOptions<PucharDbContext> options) : base(options) { }

  public DbSet<User> Users => Set<User>();
  public DbSet<Tournament> Tournaments => Set<Tournament>();
  public DbSet<Bracket> Brackets => Set<Bracket>();
  public DbSet<Match> Matches => Set<Match>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>(e =>
    {
      e.HasKey(x => x.Id);
      e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
      e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
      e.Property(x => x.Email).IsRequired().HasMaxLength(200);
      e.HasIndex(x => x.Email).IsUnique(); // ⭐ przyda się do logowania
    });

    modelBuilder.Entity<Tournament>(e =>
    {
      e.HasKey(x => x.Id);
      e.Property(x => x.Name).IsRequired().HasMaxLength(200);
      e.Property(x => x.Status).IsRequired().HasMaxLength(20);

      e.HasOne(x => x.Bracket)
           .WithOne()
           .HasForeignKey<Bracket>("TournamentId")
           .OnDelete(DeleteBehavior.Cascade);

      e.HasMany(x => x.Participants)
           .WithMany()
           .UsingEntity(j => j.ToTable("TournamentParticipants"));
    });

    modelBuilder.Entity<Bracket>(e =>
    {
      e.HasKey(x => x.Id);
      e.HasMany(x => x.Matches)
           .WithOne()
           .HasForeignKey("BracketId")
           .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<Match>(e =>
    {
      e.HasKey(x => x.Id);

      e.Property(x => x.Round).IsRequired();

      e.HasOne(x => x.Player1)
           .WithMany()
           .HasForeignKey("Player1Id")
           .OnDelete(DeleteBehavior.Restrict);

      e.HasOne(x => x.Player2)
           .WithMany()
           .HasForeignKey("Player2Id")
           .OnDelete(DeleteBehavior.Restrict);

      e.HasOne(x => x.Winner)
           .WithMany()
           .HasForeignKey("WinnerId")
           .OnDelete(DeleteBehavior.Restrict);
    });
  }
}

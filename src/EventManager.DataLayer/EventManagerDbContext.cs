using EventManager.Datalayer.Dbos;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Datalayer
{
  /// <summary>
  /// Kontekst bazy danych dla aplikacji EventManager.
  /// </summary>
  public class EventManagerDbContext : DbContext
  {
    /// <summary>
    /// Konstruktor z opcjami konfiguracji.
    /// </summary>
    public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options) { }

    /// <summary>
    /// Tabela adresów.
    /// </summary>
    public DbSet<AddressDbo> Addresses { get; set; }

    /// <summary>
    /// Tabela wydarzeń.
    /// </summary>
    public DbSet<EventDbo> Events { get; set; }

    /// <summary>
    /// Tabela użytkowników.
    /// </summary>
    public DbSet<UserDbo> Users { get; set; }

    /// <summary>
    /// Tabela relacji użytkownik-wydarzenie.
    /// </summary>
    public DbSet<UserEventDbo> UserEvents { get; set; }

    /// <summary>
    /// Konfiguracja modelu w bazie danych.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<UserEventDbo>()
          .HasKey(ue => new { ue.UserId, ue.EventId });

      modelBuilder.Entity<EventDbo>()
          .HasOne<AddressDbo>()
          .WithMany()
          .HasForeignKey(e => e.AddressId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<EventDbo>()
        .Property(e => e.Cost)
        .HasPrecision(18, 2);

      modelBuilder.Entity<UserEventDbo>()
          .Property(ue => ue.DepositPaid)
          .HasPrecision(18, 2);
    }
  }
}

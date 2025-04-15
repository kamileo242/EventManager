using System;

namespace EventManager.Models
{
  /// <summary>
  /// Reprezentacja modelu wydarzenia.
  /// </summary>
  public class Event : IEntity
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nazwa.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Opis.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data rozpoczęcia.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data zakończenia
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Identyfikator adresu.
    /// </summary>
    public Guid? AddressId { get; set; }

    /// <summary>
    /// Maksymalna ilość użytkowników.
    /// </summary>
    public int? MaxParticipants { get; set; }

    /// <summary>
    /// Cena wstępu.
    /// </summary>
    public decimal? Cost { get; set; }
  }
}

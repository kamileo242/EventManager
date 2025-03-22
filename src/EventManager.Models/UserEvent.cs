using System;

namespace EventManager.Models
{
  /// <summary>
  /// Reprezentacja tabeli łączącej wydarzenia z użytkownikami.
  /// </summary>
  public class UserEvent
  {
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Identyfikator wydarzenia.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Suma wpłacona przez użytkownika.
    /// </summary>
    public decimal DepositPaid { get; set; }

    /// <summary>
    /// Data dołączenia do wydarzenia.
    /// </summary>
    public DateTime JoinedAt { get; set; }
  }
}

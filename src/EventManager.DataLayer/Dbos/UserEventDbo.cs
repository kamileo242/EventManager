using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Datalayer.Dbos
{
  /// <summary>
  /// Reprezentacja tabeli łączącej wydarzenia z użytkownikami w bazie danych.
  /// </summary>
  [Table("uzytkownik_wydarzenie")]
  public class UserEventDbo
  {
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    [Key, Column("id_uzytkownika")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Identyfikator wydarzenia.
    /// </summary>
    [Key, Column("id_wydarzenia")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Suma wpłacona przez użytkownika.
    /// </summary>
    [Column("wplacona_kwota")]
    public decimal DepositPaid { get; set; }

    /// <summary>
    /// Data dołączenia do wydarzenia.
    /// </summary>
    [Column("data_dolaczenia")]
    public DateTime JoinedAt { get; set; }
  }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Datalayer.Dbos
{
  /// <summary>
  /// Reprezentacja modelu wydarzenia w bazie danych.
  /// </summary>
  [Table("wydarzenie")]
  public class EventDbo
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Nazwa.
    /// </summary>
    [Column("nazwa")]
    public string Name { get; set; }

    /// <summary>
    /// Opis.
    /// </summary>
    [Column("opis")]
    public string Description { get; set; }

    /// <summary>
    /// Data rozpoczęcia.
    /// </summary>
    [Column("data_rozpoczecia")]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data zakończenia.
    /// </summary>
    [Column("data_zakonczenia")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Identyfikator adresu.
    /// </summary>
    [Column("id_adresu")]
    public Guid? AddressId { get; set; }

    /// <summary>
    /// Maksymalna ilość użytkowników.
    /// </summary>
    [Column("maksymalna_liczba_uczestnikow")]
    public int? MaxParticipants { get; set; }

    /// <summary>
    /// Cena wstępu.
    /// </summary>
    [Column("koszt")]
    public decimal? Cost { get; set; }
  }
}

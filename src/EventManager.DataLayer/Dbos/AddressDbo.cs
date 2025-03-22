using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Datalayer.Dbos
{
  /// <summary>
  /// Reprezentacja modelu adresu w bazie danych.
  /// </summary>
  [Table("adres")]
  public class AddressDbo
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Miasto.
    /// </summary>
    [Column("miasto")]
    public string City { get; set; }

    /// <summary>
    /// Ulica.
    /// </summary>
    [Column("ulica")]
    public string Street { get; set; }

    /// <summary>
    /// Numer domu.
    /// </summary>
    [Column("numer_domu")]
    public string HouseNumber { get; set; }

    /// <summary>
    /// Lista wydarzeń w danym miejscu.
    /// </summary>
    [Column("wydarzenia")]
    public Guid[] EventsIds { get; set; }
  }
}

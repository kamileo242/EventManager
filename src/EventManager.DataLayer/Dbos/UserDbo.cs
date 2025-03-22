using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Datalayer.Dbos
{
  /// <summary>
  /// Reprezentacja modelu użytkownika w bazie danych.
  /// </summary>
  [Table("uzytkownik")]
  public class UserDbo
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Imię.
    /// </summary>
    [Column("imie")]
    public string Name { get; set; }

    /// <summary>
    /// Nazwisko.
    /// </summary>
    [Column("nazwisko")]
    public string LastName { get; set; }
  }
}

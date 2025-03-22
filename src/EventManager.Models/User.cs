using System;

namespace EventManager.Models
{
  /// <summary>
  /// Reprezentacja modelu użytkownika.
  /// </summary>
  public class User : IEntity
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Imię.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Nazwisko.
    /// </summary>
    public string LastName { get; set; }
  }
}

using System;

namespace EventManager.Models
{
  /// <summary>
  /// Interfejs oznaczający klasę encji, która musi posiadać właściwość Id.
  /// Każda klasa encji, która implementuje ten interfejs, musi zawierać unikalny identyfikator (Guid) dla obiektów.
  /// </summary>
  public interface IEntity
  {
    /// <summary>
    /// Unikalny identyfikator encji (np. w bazie danych).
    /// </summary>
    Guid Id { get; set; }
  }
}

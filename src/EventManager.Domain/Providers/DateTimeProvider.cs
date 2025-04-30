using System;

namespace EventManager.Domain.Providers
{
  /// <summary>
  /// Klasa generująca aktualny czas
  /// </summary>
  public static class DateTimeProvider
  {
    /// <summary>
    /// Metoda generująca aktualny czas
    /// </summary>
    /// <returns>Aktualny czas</returns>
    public static DateTime GetTime()
        => DateTime.Now;
  }
}

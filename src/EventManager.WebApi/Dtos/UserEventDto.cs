namespace EventManager.WebApi.Dtos
{
  /// <summary>
  /// Reprezentacja tabeli łączącej wydarzenia z użytkownikami.
  /// </summary>
  public record UserEventDto
  {
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public UserDto User { get; init; }

    /// <summary>
    /// Identyfikator wydarzenia.
    /// </summary>
    public EventDto Event { get; init; }

    /// <summary>
    /// Suma wpłacona przez użytkownika.
    /// </summary>
    public decimal DepositPaid { get; init; }

    /// <summary>
    /// Data dołączenia do wydarzenia.
    /// </summary>
    public DateTime JoinedAt { get; init; }
  }
}

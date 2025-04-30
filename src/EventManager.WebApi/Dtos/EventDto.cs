namespace EventManager.WebApi.Dtos
{
  /// <summary>
  /// Reprezentacja modelu dto wydarzenia.
  /// </summary>
  public record EventDto
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Nazwa.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Opis.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Data rozpoczęcia.
    /// </summary>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Data zakończenia
    /// </summary>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Identyfikator adresu.
    /// </summary>
    public AddressDto? Address { get; init; }

    /// <summary>
    /// Maksymalna ilość użytkowników.
    /// </summary>
    public int? MaxParticipants { get; init; }

    /// <summary>
    /// Cena wstępu.
    /// </summary>
    public decimal? Cost { get; init; }
  }
}

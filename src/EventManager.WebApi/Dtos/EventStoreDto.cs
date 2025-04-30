namespace EventManager.WebApi.Dtos
{
  /// <summary>
  /// Reprezentacja modelu dto dodawanego oraz aktualizowanego wydarzenia.
  /// </summary>
  public record EventStoreDto
  {
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
    /// Maksymalna ilość użytkowników.
    /// </summary>
    public int? MaxParticipants { get; init; }

    /// <summary>
    /// Cena wstępu.
    /// </summary>
    public decimal? Cost { get; init; }
  }
}

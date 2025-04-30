namespace EventManager.WebApi.Dtos
{
  /// <summary>
  /// Reprezentacja modelu dto dodawanego oraz aktualizowanego użytkownika.
  /// </summary>
  public record UserStoreDto
  {
    /// <summary>
    /// Imię.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Nazwisko.
    /// </summary>
    public string LastName { get; init; }
  }
}

namespace EventManager.WebApi.Dtos
{
  /// <summary>
  /// Reprezentacja modelu dto użytkownika.
  /// </summary>
  public record UserDto
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    public string Id { get; init; }

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

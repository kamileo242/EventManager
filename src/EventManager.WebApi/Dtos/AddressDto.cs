namespace EventManager.WebApi.Dtos
{
  /// <summary>
  /// Reprezentacja modelu dto adresu.
  /// </summary>
  public record AddressDto
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Miasto.
    /// </summary>
    public string City { get; init; }

    /// <summary>
    /// Ulica.
    /// </summary>
    public string Street { get; init; }

    /// <summary>
    /// Numer domu.
    /// </summary>
    public string HouseNumber { get; init; }

    /// <summary>
    /// Lista wydarzeń w danym miejscu.
    /// </summary>
    public List<EventDto> Events { get; init; }
  }
}

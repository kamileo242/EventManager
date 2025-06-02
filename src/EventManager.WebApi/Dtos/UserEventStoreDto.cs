namespace EventManager.WebApi.Dtos
{
    /// <summary>
    /// Reprezentacja modelu dto wzięcia udziału w wydarzeniu.
    /// </summary>
    public record UserEventStoreDto
    {
        /// <summary>
        /// Identyfikator użytkownika.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Identyfikator wydarzenia.
        /// </summary>
        public string EventId { get; init; }
    }
}

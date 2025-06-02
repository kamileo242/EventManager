namespace EventManager.WebApi.Dtos
{
    /// <summary>
    /// Reprezentacja modelu dto dodawanego oraz aktualizowanego adresu.
    /// </summary>
    public record AddressStoreDto
    {
        /// <summary>
        /// Miasto.
        /// </summary>
        public string? City { get; init; }

        /// <summary>
        /// Ulica.
        /// </summary>
        public string? Street { get; init; }

        /// <summary>
        /// Numer domu.
        /// </summary>
        public string? HouseNumber { get; init; }
    }
}

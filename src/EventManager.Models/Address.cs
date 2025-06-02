using System;

namespace EventManager.Models
{
    /// <summary>
    /// Reprezentacja modelu adresu.
    /// </summary>
    public class Address : IEntity
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Miasto.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Ulica.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Numer domu.
        /// </summary>
        public string HouseNumber { get; set; }
    }
}

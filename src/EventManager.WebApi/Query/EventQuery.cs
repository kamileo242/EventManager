using System.Linq.Expressions;
using EventManager.Models;
using EventManager.WebApi.Converts;

namespace EventManager.WebApi.Query
{
    /// <summary>
    /// Model zawierający właściwości do wyszukiwania wydarzeń.
    /// </summary>
    public class EventQuery
    {
        /// <summary>
        /// Fragment nazwy wydarzenia (filtrowanie po Contains).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Fragment opisu wydarzenia (filtrowanie po Contains).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Minimalna data rozpoczęcia wydarzenia.
        /// </summary>
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        /// Maksymalna data rozpoczęcia wydarzenia.
        /// </summary>
        public DateTime? StartDateTo { get; set; }

        /// <summary>
        /// Minimalna data zakończenia wydarzenia.
        /// </summary>
        public DateTime? EndDateFrom { get; set; }

        /// <summary>
        /// Maksymalna data zakończenia wydarzenia.
        /// </summary>
        public DateTime? EndDateTo { get; set; }

        /// <summary>
        /// Identyfikator adresu wydarzenia (jako string).
        /// </summary>
        public string? AddressId { get; set; }

        /// <summary>
        /// Minimalna dopuszczalna liczba uczestników wydarzenia.
        /// </summary>
        public int? MinParticipants { get; set; }

        /// <summary>
        /// Maksymalna dopuszczalna liczba uczestników wydarzenia.
        /// </summary>
        public int? MaxParticipants { get; set; }

        /// <summary>
        /// Minimalny koszt uczestnictwa.
        /// </summary>
        public decimal? MinCost { get; set; }

        /// <summary>
        /// Maksymalny koszt uczestnictwa.
        /// </summary>
        public decimal? MaxCost { get; set; }

        /// <summary>
        /// Buduje wyrażenie filtrujące na podstawie podanych parametrów.
        /// </summary>
        public Expression<Func<Event, bool>> ToExpression()
        {
            var addressGuid = AddressId?.TextToGuidOrNull();

            return e =>
                (string.IsNullOrEmpty(Name) || e.Name.Contains(Name)) &&
                (string.IsNullOrEmpty(Description) || e.Description.Contains(Description)) &&
                (!StartDateFrom.HasValue || e.StartDate >= StartDateFrom) &&
                (!StartDateTo.HasValue || e.StartDate <= StartDateTo) &&
                (!EndDateFrom.HasValue || e.EndDate >= EndDateFrom) &&
                (!EndDateTo.HasValue || e.EndDate <= EndDateTo) &&
                (!addressGuid.HasValue || e.AddressId == addressGuid.Value) &&
                (!MinParticipants.HasValue || e.MaxParticipants >= MinParticipants) &&
                (!MaxParticipants.HasValue || e.MaxParticipants <= MaxParticipants) &&
                (!MinCost.HasValue || e.Cost >= MinCost) &&
                (!MaxCost.HasValue || e.Cost <= MaxCost);
        }
    }
}

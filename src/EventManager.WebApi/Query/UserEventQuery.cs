using System.Linq.Expressions;
using EventManager.Models;
using EventManager.WebApi.Converts;

namespace EventManager.WebApi.Query
{
    /// <summary>
    /// Model zawierający właściwości do wyszukania powiązań użytkowników z wydarzeniami.
    /// </summary>
    public class UserEventQuery
    {
        /// <summary>
        /// Identyfikator użytkownika.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Identyfikator wydarzenia.
        /// </summary>
        public string? EventId { get; set; }

        /// <summary>
        /// Minimalna suma wpłacona przez użytkownika.
        /// </summary>
        public decimal? MinDepositPaid { get; set; }

        /// <summary>
        /// Maksymalna suma wpłacona przez użytkownika.
        /// </summary>
        public decimal? MaxDepositPaid { get; set; }

        /// <summary>
        /// Data dołączenia od.
        /// </summary>
        public DateTime? JoinedFrom { get; set; }

        /// <summary>
        /// Data dołączenia do.
        /// </summary>
        public DateTime? JoinedTo { get; set; }

        /// <summary>
        /// Tworzy wyrażenie filtrujące dane UserEvent na podstawie podanych kryteriów.
        /// </summary>
        public Expression<Func<UserEvent, bool>> ToExpression()
        {
            var userGuid = UserId?.TextToGuidOrNull();
            var eventGuid = EventId?.TextToGuidOrNull();

            return ue =>
                (!userGuid.HasValue || ue.UserId == userGuid.Value) &&
                (!eventGuid.HasValue || ue.EventId == eventGuid.Value) &&
                (!MinDepositPaid.HasValue || ue.DepositPaid >= MinDepositPaid) &&
                (!MaxDepositPaid.HasValue || ue.DepositPaid <= MaxDepositPaid) &&
                (!JoinedFrom.HasValue || ue.JoinedAt >= JoinedFrom) &&
                (!JoinedTo.HasValue || ue.JoinedAt <= JoinedTo);
        }
    }
}

using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    /// <summary>
    /// Правила описывающие поведение конторы.
    /// </summary>
    public class NodeRules
    {
        /// <summary>
        /// Внешний идентификатор конторы, например "k1".
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Набор принимаемых и выдаваемых конторой справок.
        /// </summary>
        public ICollection<InOut> InOuts { get; set; }
    }
}
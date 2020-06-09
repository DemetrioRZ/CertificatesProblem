using System;
using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    /// <summary>
    /// Используется для описания конторы, принимаемые справки и выдаваемая по ним справка.
    /// </summary>
    public class InOut
    {
        /// <summary>
        /// Требуемые на вход справки.
        /// </summary>
        public ICollection<Certificate> RequiredInputs { get; set; }

        /// <summary>
        /// Выдаваемая справка.
        /// </summary>
        public Certificate Output { get; set; }

        /// <summary>
        /// Затраты времени на выдачу справки.
        /// </summary>
        public TimeCost TimeCost { get; set; }

        /// <summary>
        /// Затраты денег на выдачу справки.
        /// </summary>
        public decimal MoneyCost { get; set; }
    }
}
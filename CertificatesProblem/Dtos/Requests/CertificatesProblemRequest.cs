using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    /// <summary>
    /// Запрос на решение задачи о сертификатах
    /// </summary>
    public class CertificatesProblemRequest
    {
        /// <summary>
        /// Правила описывающие работу контор.
        /// </summary>
        public ICollection<NodeRules> NodesRules { get; set; }

        /// <summary>
        /// Набор справок, который необходимо получить.
        /// </summary>
        public ICollection<Certificate> TargetCertificates { get; set; }

        /// <summary>
        /// Набор уже имеющихся справок.
        /// </summary>
        public ICollection<ExistingCertificate> ExistingCertificates { get; set; }

        /// <summary>
        /// Стратегия решения задачи.
        /// </summary>
        public StrategyRequest Strategy { get; set; }
    }
}
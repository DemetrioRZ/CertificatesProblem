using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    public class CertificatesProblemRequest
    {
        public ICollection<NodeRules> NodesRules { get; set; }

        public ICollection<TargetCertificate> TargetCertificates { get; set; }

        public ICollection<ExistingCertificate> ExistingCertificates { get; set; }

        public StrategyRequest Strategy { get; set; }
    }
}
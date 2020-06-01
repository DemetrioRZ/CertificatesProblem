using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    public class CertificatesProblemRequest
    {
        public ICollection<NodeRules> NodesRules { get; set; }

        public ICollection<Certificate> TargetCertificates { get; set; }
    }
}
using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    public class NodeRules
    {
        public string NodeId { get; set; }

        public ICollection<InOut> InOuts { get; set; }
    }
}
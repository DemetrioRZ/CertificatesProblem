using System.Collections.Generic;
using CertificatesProblem.Model;

namespace CertificatesProblem.Interfaces
{
    public interface ICertificatesProblemService
    {
        ICollection<Node> Solve(ICollection<NodeDescription> nodeDescriptions, ICollection<string> targetCertificates, Strategy strategy);
    }
}
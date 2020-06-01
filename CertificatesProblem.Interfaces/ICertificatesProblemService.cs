using System.Collections.Generic;
using CertificatesProblem.Model;

namespace CertificatesProblem.Interfaces
{
    public interface ICertificatesProblemService
    {
        string Solve(ICollection<NodeDescription> nodeDescriptions, ICollection<string> targetCertificates);
    }
}
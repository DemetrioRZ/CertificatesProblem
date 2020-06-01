using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Dtos.Requests;
using CertificatesProblem.Interfaces.Mappers;

namespace CertificatesProblem.Mappers
{
    public class TargetCertificatesMapper : IMapper<IEnumerable<Certificate>, IEnumerable<string>>
    {
        public IEnumerable<string> Map(IEnumerable<Certificate> targetCertificates)
        {
            return targetCertificates.Select(x => x.CertificateId);
        }
    }
}
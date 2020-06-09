using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Dtos.Requests;
using CertificatesProblem.Interfaces.Mappers;

namespace CertificatesProblem.Mappers
{
    public class TargetCertificatesMapper : IMapper<ICollection<Certificate>, ICollection<string>>
    {
        public ICollection<string> Map(ICollection<Certificate> targetCertificates)
        {
            return targetCertificates.Select(x => x.CertificateId).ToList();
        }
    }
}
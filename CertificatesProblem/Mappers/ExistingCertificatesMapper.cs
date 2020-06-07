using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Dtos.Requests;
using CertificatesProblem.Interfaces.Mappers;

namespace CertificatesProblem.Mappers
{
    public class ExistingCertificatesMapper : IMapper<ICollection<ExistingCertificate>, ICollection<string>>
    {
        public ICollection<string> Map(ICollection<ExistingCertificate> existingCertificates)
        {
            return existingCertificates?.Select(x => x.CertificateId).ToList() ?? new List<string>();
        }
    }
}
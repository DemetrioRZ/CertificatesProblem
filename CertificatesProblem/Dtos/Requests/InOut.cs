using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    public class InOut
    {
        public ICollection<Certificate> RequiredInputs { get; set; }

        public Certificate Output { get; set; }
    }
}
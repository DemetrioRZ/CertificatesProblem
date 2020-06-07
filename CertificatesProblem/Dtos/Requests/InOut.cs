using System;
using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Requests
{
    public class InOut
    {
        public ICollection<TargetCertificate> RequiredInputs { get; set; }

        public TargetCertificate Output { get; set; }

        public TimeCost TimeCost { get; set; }

        public decimal MoneyCost { get; set; }
    }
}
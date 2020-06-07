using System;
using System.Collections.Generic;
using System.Linq;

namespace CertificatesProblem.Model
{
    public class NodeDescription
    {
        public string Title { get; set; }

        

        public ICollection<string> Inputs { get; set; }

        public string Output { get; set; }

        public TimeSpan TimeCost { get; set; }

        public decimal MoneyCost { get; set; }

        public string UniqueSignature => Inputs != null && Inputs.Any() 
            ? $"{string.Join(",", Inputs)}->{Title}->{Output}"
            : $"{Title}->{Output}";
    }
}

using System.Collections.Generic;

namespace CertificatesProblem.Dtos.Results
{
    public class CertificatesProblemSolution
    {
        public CertificatesProblemSolution()
        {
            SolutionsAsEquation = new List<SolutionForCertificate>();
        }

        public ICollection<SolutionForCertificate> SolutionsAsEquation { get; set; }
    }
}
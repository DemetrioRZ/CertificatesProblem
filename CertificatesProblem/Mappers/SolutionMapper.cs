using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Dtos.Results;
using CertificatesProblem.Interfaces.Mappers;
using CertificatesProblem.Model;

namespace CertificatesProblem.Mappers
{
    public class SolutionMapper : IMapper<ICollection<Node>, CertificatesProblemSolution>
    {
        public CertificatesProblemSolution Map(ICollection<Node> rootNodes)
        {
            var result = new CertificatesProblemSolution();

            foreach (var rootNode in rootNodes.OrderBy(x => x.Description.Output))
            {
                var certificateSolution = new SolutionForCertificate { CertificateId = rootNode.Description.Output, SolutionAsEquation = rootNode.ToEquation() };
                result.SolutionsAsEquation.Add(certificateSolution);
            }

            return result;
        }
    }
}
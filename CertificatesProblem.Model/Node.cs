using System.Collections.Generic;
using System.Linq;

namespace CertificatesProblem.Model
{
    public class Node
    {
        public NodeDescription Description { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node Parent { get; set; }

        public string ToFormula(bool isFullEquation = true)
        {
            var subNodes = Children != null ? string.Join(", ", Children?.Select(x => x.ToFormula(false))) : string.Empty;
            var rightPartOfEquation = isFullEquation ? $" = {Description.Output}" : string.Empty;
            var formula = $"{Description.Title}<out:{Description.Output}>({subNodes}){rightPartOfEquation}";

            return formula;
        }
    }
}
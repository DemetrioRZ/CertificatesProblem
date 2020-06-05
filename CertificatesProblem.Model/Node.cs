using System.Collections.Generic;
using System.Linq;

namespace CertificatesProblem.Model
{
    public class Node
    {
        public NodeDescription Description { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node Parent { get; set; }

        public string ToEquation(bool isFullEquation = true)
        {
            var subNodes = Children != null ? string.Join(", ", Children?.Select(x => x.ToEquation(false))) : string.Empty;
            var rightPartOfEquation = isFullEquation ? $" = {Description.Output}" : string.Empty;
            var formula = $"{Description.Title}<out:{Description.Output}>({subNodes}){rightPartOfEquation}";

            return formula;
        }

        public int GetTotalNodesCount()
        {
            return 1 + (Children?.Sum(x => x.GetTotalNodesCount()) ?? 0);
        }

        public Node SearchParentCyclicReferences(HashSet<string> uniqueNodes = null)
        {
            if (uniqueNodes == null)
                uniqueNodes = new HashSet<string>();

            if (!uniqueNodes.Contains(Description.UniqueSignature))
            {
                uniqueNodes.Add(Description.UniqueSignature);
                return Parent?.SearchParentCyclicReferences(uniqueNodes);
            }

            return this;
        }
    }
}
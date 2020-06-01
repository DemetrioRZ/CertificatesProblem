using System.Collections.Generic;

namespace CertificatesProblem.Model
{
    public class Node
    {
        public NodeDescription Description { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node Parent { get; set; }
    }
}
using System.Collections.Generic;

namespace CertificatesProblem.Model
{
    public class Node
    {
        public NodeDescription Description { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node Parent { get; set; }

        public Node Clone()
        {
            var clone = new Node();

            clone.Description = Description;
            clone.Parent = Parent?.Clone();
            clone.Children = new List<Node>();
            foreach (var child in Children)
            {
                clone.Children
            }
        }
    }
}
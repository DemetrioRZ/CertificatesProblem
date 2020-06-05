using System;
using System.Collections.Generic;
using System.Linq;

namespace CertificatesProblem.Model
{
    public class Node
    {
        public Node()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

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

        public TimeSpan GetTotalTimeCostSerialVisits()
        {
            var childrenTimeCost = new TimeSpan();
            if (Children != null)
                foreach (var child in Children)
                    childrenTimeCost = childrenTimeCost + child.GetTotalTimeCostSerialVisits();

            return Description.TimeCost + childrenTimeCost;
        }

        public decimal GetTotalMoneyCost()
        {
            return Description.MoneyCost + (Children?.Sum(x => x.Description.MoneyCost) ?? 0);
        }

        public TimeSpan GetTotalTimeCostParallelVisits()
        {
            var startNodes = GetStartNodesForRoot();
            return startNodes.Max(x => x.GetTotalTimeCostForBranch());
        }

        private TimeSpan GetTotalTimeCostForBranch()
        {
            return Parent != null 
                ? Description.TimeCost + Parent.GetTotalTimeCostForBranch()
                : Description.TimeCost;
        }

        public ICollection<Node> GetStartNodesForRoot()
        {
            var startNodes = new List<Node>();

            if (Children != null)
                foreach (var child in Children)
                    startNodes.AddRange(child.GetStartNodesForRoot());
            
            if (Children == null)
                startNodes.Add(this);

            return startNodes;
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
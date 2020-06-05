using System.Collections.Generic;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessNodesToVisitStrategy : IComparisonStrategy, IComparer<Node>
    {
        public IComparer<Node> GetComparer()
        {
            return this;
        }

        public Strategy Strategy => Strategy.LessNodesToVisit;

        public int Compare(Node x, Node y)
        {
            var xNodesCount = x.GetTotalNodesCount();
            var yNodesCount = y.GetTotalNodesCount();
            return xNodesCount.CompareTo(yNodesCount);
        }
    }
}
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessNodesToVisitStrategy : IComparisonStrategy
    {
        public Strategy Strategy => Strategy.LessNodesToVisit;

        public int Compare(Node x, Node y)
        {
            var xNodesCount = x.GetTotalNodesCount();
            var yNodesCount = y.GetTotalNodesCount();
            return xNodesCount.CompareTo(yNodesCount);
        }
    }
}
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessTimeCostParallelVisitsStrategy : IComparisonStrategy
    {
        public int Compare(Node x, Node y)
        {
            var xNodesCount = x.GetTotalTimeCostParallelVisits();
            var yNodesCount = y.GetTotalTimeCostParallelVisits();
            return xNodesCount.CompareTo(yNodesCount);
        }

        public Strategy Strategy => Strategy.LessTimeCostParallelVisits;
    }
}
using System.Collections.Generic;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessTimeCostParallelVisitsStrategy : IComparisonStrategy, IComparer<Node>
    {
        public IComparer<Node> GetComparer()
        {
            return this;
        }

        public int Compare(Node x, Node y)
        {
            var xNodesCount = x.GetTotalTimeCostParallelVisits();
            var yNodesCount = y.GetTotalTimeCostParallelVisits();
            return xNodesCount.CompareTo(yNodesCount);
        }

        public Strategy Strategy => Strategy.LessTimeCostParallelVisits;
    }
}
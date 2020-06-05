using System.Collections.Generic;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessTimeCostSerialVisitsStrategy : IComparisonStrategy, IComparer<Node>
    {
        public IComparer<Node> GetComparer()
        {
            return this;
        }

        public int Compare(Node x, Node y)
        {
            var xNodesCount = x.GetTotalTimeCostSerialVisits();
            var yNodesCount = y.GetTotalTimeCostSerialVisits();
            return xNodesCount.CompareTo(yNodesCount);
        }
        
        public Strategy Strategy => Strategy.LessTimeCostSerialVisits;
    }
}
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessMoneyCostStrategy : IComparisonStrategy
    {
        public int Compare(Node x, Node y)
        {
            var xNodesCount = x.GetTotalMoneyCost();
            var yNodesCount = y.GetTotalMoneyCost();
            return xNodesCount.CompareTo(yNodesCount);
        }

        public Strategy Strategy => Strategy.LessMoneyCost;
    }
}
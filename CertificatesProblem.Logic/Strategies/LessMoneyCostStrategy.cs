using System.Collections.Generic;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessMoneyCostStrategy : IComparisonStrategy, IComparer<Node>
    {
        public IComparer<Node> GetComparer()
        {
            throw new System.NotImplementedException();
        }

        public int Compare(Node x, Node y)
        {
            throw new System.NotImplementedException();
        }

        public Strategy Strategy => Strategy.LessMoneyCost;
    }
}
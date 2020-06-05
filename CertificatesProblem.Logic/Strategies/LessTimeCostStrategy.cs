using System.Collections.Generic;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class LessTimeCostStrategy : IComparisonStrategy, IComparer<Node>
    {
        public IComparer<Node> GetComparer()
        {
            return this;
        }

        public int Compare(Node x, Node y)
        {
            throw new System.NotImplementedException();
        }
        
        public Strategy Strategy => Strategy.LessTimeCost;
    }
}
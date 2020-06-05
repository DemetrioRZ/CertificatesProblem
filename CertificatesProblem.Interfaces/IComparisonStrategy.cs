using System.Collections.Generic;
using CertificatesProblem.Model;

namespace CertificatesProblem.Interfaces
{
    public interface IComparisonStrategy : IComparer<Node>
    {
        Strategy Strategy { get; }
    }
}
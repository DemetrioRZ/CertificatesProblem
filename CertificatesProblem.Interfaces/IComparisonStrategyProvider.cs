using CertificatesProblem.Model;

namespace CertificatesProblem.Interfaces
{
    public interface IComparisonStrategyProvider
    {
        IComparisonStrategy GetComparisonStrategy(Strategy strategy);
    }
}
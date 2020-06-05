using System;
using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Strategies
{
    public class ComparisonStrategyProvider : IComparisonStrategyProvider
    {
        private readonly ICollection<IComparisonStrategy> _comparisonStrategies;
        private Dictionary<Strategy, IComparisonStrategy> _comparisonStrategiesDictionary;

        public ComparisonStrategyProvider(ICollection<IComparisonStrategy> comparisonStrategies)
        {
            _comparisonStrategies = comparisonStrategies;
        }

        public IComparisonStrategy GetComparisonStrategy(Strategy strategy)
        {
            if (_comparisonStrategiesDictionary == null)
                _comparisonStrategiesDictionary = _comparisonStrategies.ToDictionary(x => x.Strategy, y => y);

            if (_comparisonStrategiesDictionary.ContainsKey(strategy))
                return _comparisonStrategiesDictionary[strategy];

            throw new ArgumentOutOfRangeException(strategy.ToString());
        }
    }
}
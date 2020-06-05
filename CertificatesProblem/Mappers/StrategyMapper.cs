using System;
using CertificatesProblem.Dtos.Requests;
using CertificatesProblem.Interfaces.Mappers;
using CertificatesProblem.Model;

namespace CertificatesProblem.Mappers
{
    public class StrategyMapper : IMapper<StrategyRequest, Strategy>
    {
        public Strategy Map(StrategyRequest strategyRequest)
        {
            if (Enum.TryParse<Strategy>(strategyRequest.ToString(), out var result))
                return result;

            throw new ArgumentOutOfRangeException(strategyRequest.ToString());
        }
    }
}
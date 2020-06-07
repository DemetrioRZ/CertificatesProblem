using Autofac;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Logic;
using CertificatesProblem.Logic.Strategies;

namespace CertificatesProblem.IoC
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CertificatesProblemService>().As<ICertificatesProblemService>();
            builder.RegisterType<ComparisonStrategyProvider>().As<IComparisonStrategyProvider>();
            
            builder.RegisterType<LessNodesToVisitStrategy>().As<IComparisonStrategy>();
            builder.RegisterType<LessTimeCostSerialVisitsStrategy>().As<IComparisonStrategy>();
            builder.RegisterType<LessTimeCostParallelVisitsStrategy>().As<IComparisonStrategy>();
            builder.RegisterType<LessMoneyCostStrategy>().As<IComparisonStrategy>();
        }
    }
}
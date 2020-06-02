using Autofac;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Logic;

namespace CertificatesProblem.IoC
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CertificatesProblemService>().As<ICertificatesProblemService>();
        }
    }
}
﻿using Autofac;
using CertificatesProblem.Mappers;

namespace CertificatesProblem.IoC
{
    public class MappingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NodeRulesMapper>().AsImplementedInterfaces();
            builder.RegisterType<TargetCertificatesMapper>().AsImplementedInterfaces();
            builder.RegisterType<ExistingCertificatesMapper>().AsImplementedInterfaces();
            builder.RegisterType<SolutionMapper>().AsImplementedInterfaces();
            builder.RegisterType<StrategyMapper>().AsImplementedInterfaces();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<NodeDescription> GetNodeDescriptionsWithOutput(this IEnumerable<NodeDescription> nodeDescriptions, string outputCertificate)
        {
            return nodeDescriptions.Where(x => string.Equals(x.Output, outputCertificate, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
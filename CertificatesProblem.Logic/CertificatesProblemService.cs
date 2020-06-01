using System;
using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Logic.Extensions;
using CertificatesProblem.Model;

namespace CertificatesProblem.Logic
{
    public class CertificatesProblemService : ICertificatesProblemService
    {
        public string Solve(ICollection<NodeDescription> nodeDescriptions, ICollection<string> targetCertificates)
        {
            foreach (var targetCertificate in targetCertificates)
            {
                var rootNodes = GetPossibleNodesForTarget(targetCertificate, nodeDescriptions);

                var rootNode = rootNodes.Count > 1 ? GetRootNodeWithMinTransitions(rootNodes) : rootNodes.Single();


            }

            return string.Empty;
        }

        private ICollection<Node> GetPossibleNodesForTarget(string target, ICollection<NodeDescription> nodeDescriptions, Node parentNode = null)
        {
            var result = new List<Node>();

            var targetDescriptions = nodeDescriptions.GetNodeDescriptionsWithOutput(target);

            foreach (var targetDescription in targetDescriptions)
            {
                var node = new Node {Description = targetDescription, Parent = parentNode};
                result.Add(node);

                var subTargets = GetTargetsForNode(node);

                foreach (var subTarget in subTargets)
                {
                    GetPossibleNodesForTarget(subTarget, nodeDescriptions, node);
                }
            }

            return result;
        }

        private ICollection<string> GetTargetsForNode(Node node)
        {
            return node.Description.Inputs ?? new List<string>();
        }

        private Node GetRootNodeWithMinTransitions(ICollection<Node> rootNodes)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;
using CertificatesProblem.Model.Exceptions;

namespace CertificatesProblem.Logic
{
    public class CertificatesProblemService : ICertificatesProblemService
    {
        private readonly IComparisonStrategyProvider _comparisonStrategyProvider;

        public CertificatesProblemService(IComparisonStrategyProvider comparisonStrategyProvider)
        {
            _comparisonStrategyProvider = comparisonStrategyProvider;
        }

        public ICollection<Node> Solve(ICollection<NodeDescription> nodeDescriptions, ICollection<string> targetCertificates, ICollection<string> existingCertificates, Strategy strategy)
        {
            var rootNodes = new List<Node>();

            foreach (var targetCertificate in targetCertificates)
            {
                var rootNode = GetNextNode(targetCertificate, nodeDescriptions, strategy);

                rootNodes.Add(rootNode);
            }

            UseExistingCertificates(rootNodes, existingCertificates, strategy);
            
            return rootNodes;
        }

        private Node GetNextNode(string targetCertificate, ICollection<NodeDescription> nodeDescriptions, Strategy strategy, Node parentNode = null)
        {
            var nextNodeDescriptions = nodeDescriptions.Where(x => string.Equals(x.Output, targetCertificate, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!nextNodeDescriptions.Any())
                throw new CanNotBeSolvedException($"нет подходящего {nameof(NodeDescription)} для {nameof(targetCertificate)} = {targetCertificate}");
            
            if (nextNodeDescriptions.Count > 1)
            {
                var bestAlternativeNode = GetBestAlternative(nextNodeDescriptions, nodeDescriptions, strategy);
                bestAlternativeNode.Parent = parentNode;

                CheckCyclicReferences(bestAlternativeNode);

                return bestAlternativeNode;
            }

            var nextNode = new Node
            {
                Description = nextNodeDescriptions.Single(), 
                Parent = parentNode
            };

            FillSubNodesFor(nextNode, nodeDescriptions, strategy);
            
            CheckCyclicReferences(nextNode);

            return nextNode;
        }

        private Node GetBestAlternative(ICollection<NodeDescription> alternativesDescriptions, ICollection<NodeDescription> nodeDescriptions, Strategy strategy)
        {
            if (alternativesDescriptions.Select(x => x.Output.ToLower()).Distinct().Count() != 1)
                throw new ArgumentException(nameof(alternativesDescriptions));

            var alternatives = new List<Node>();

            foreach (var alternativesDescription in alternativesDescriptions)
            {
                var alternative = new Node
                {
                    Description = alternativesDescription, 
                    Parent = null
                };

                FillSubNodesFor(alternative, nodeDescriptions, strategy);

                alternatives.Add(alternative);
            }

            var comparisonStrategy = _comparisonStrategyProvider.GetComparisonStrategy(strategy);
            alternatives.Sort(comparisonStrategy);
            return alternatives.First(); 
        }

        private void FillSubNodesFor(Node parentNode, ICollection<NodeDescription> nodeDescriptions, Strategy strategy)
        {
            if (parentNode.Description.Inputs != null && parentNode.Description.Inputs.Any())
            {
                foreach (var parentInputCertificate in parentNode.Description.Inputs)
                {
                    var subAlternativeNode = GetNextNode(parentInputCertificate, nodeDescriptions, strategy, parentNode);
                
                    if (parentNode.Children == null)
                        parentNode.Children = new List<Node>();

                    parentNode.Children.Add(subAlternativeNode);
                }
            }
        }

        private void CheckCyclicReferences(Node newFoundNode)
        {
            var cyclicReferenceNode = newFoundNode.SearchParentCyclicReferences();

            if (cyclicReferenceNode != null)
                throw new CanNotBeSolvedException($"Найдена циклическая зависимость для {newFoundNode.Description.UniqueSignature}");
        }

        private void UseExistingCertificates(ICollection<Node> rootNodes, ICollection<string> existingCertificates, Strategy strategy)
        {
            foreach (var existingCertificate in existingCertificates)
            {
                var candidatesForReplacement =
                    rootNodes.SelectMany(x => x.GetChildNodesWithOutput(existingCertificate))
                        .Where(x => x.NodeType != NodeType.StubForExistingCertificate).ToList();

                if (!candidatesForReplacement.Any())
                    continue;

                var comparisonStrategy = _comparisonStrategyProvider.GetComparisonStrategy(strategy);
                candidatesForReplacement.Sort(comparisonStrategy);
                var nodeToReplace = candidatesForReplacement.Last();

                var stubNode = new Node
                {
                    Description = new NodeDescription{ Output = existingCertificate, Title = "Existing" }, 
                    NodeType = NodeType.StubForExistingCertificate
                };

                var parent = nodeToReplace.Parent;
                if (parent == null)
                {
                    rootNodes.Remove(nodeToReplace);
                    rootNodes.Add(stubNode);
                }
                else
                {
                    parent.Children.Remove(nodeToReplace);
                    parent.Children.Add(stubNode);
                }
            }
        }
    }
}

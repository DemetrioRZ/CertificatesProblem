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

        public ICollection<Node> Solve(ICollection<NodeDescription> nodeDescriptions, ICollection<string> targetCertificates, Strategy strategy)
        {
            var rootNodes = new List<Node>();

            foreach (var targetCertificate in targetCertificates)
            {
                var rootNode = GetNextNode(targetCertificate, nodeDescriptions, strategy);

                rootNodes.Add(rootNode);
            }

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

            if (nextNode.Description.Inputs != null && nextNode.Description.Inputs.Any())
            {
                foreach (var nextNodeInputCertificate in nextNode.Description.Inputs)
                {
                    var subNextNode = GetNextNode(nextNodeInputCertificate, nodeDescriptions, strategy, nextNode);
                
                    if (nextNode.Children == null)
                        nextNode.Children = new List<Node>();

                    nextNode.Children.Add(subNextNode);
                }
            }
            
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

                if (alternative.Description.Inputs != null && alternative.Description.Inputs.Any())
                {
                    foreach (var alternativeInputCertificate in alternative.Description.Inputs)
                    {
                        var subAlternativeNode = GetNextNode(alternativeInputCertificate, nodeDescriptions, strategy, alternative);
                
                        if (alternative.Children == null)
                            alternative.Children = new List<Node>();

                        alternative.Children.Add(subAlternativeNode);
                    }
                }

                alternatives.Add(alternative);
            }

            var comparisonStrategy = _comparisonStrategyProvider.GetComparisonStrategy(strategy);
            alternatives.Sort(comparisonStrategy.GetComparer());
            return alternatives.First(); 
        }

        private void CheckCyclicReferences(Node newFoundNode)
        {
            var cyclicReferenceNode = newFoundNode.SearchParentCyclicReferences();

            if (cyclicReferenceNode != null)
                throw new CanNotBeSolvedException($"Найдена циклическая зависимость для {newFoundNode.Description.UniqueSignature}");
        }
    }
}

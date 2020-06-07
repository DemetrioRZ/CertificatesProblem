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
                var rootNode = GetNextNode(targetCertificate, nodeDescriptions, strategy, existingCertificates);

                rootNodes.Add(rootNode);
            }

            TryResolveCyclicReferences(rootNodes, existingCertificates);

            UseExistingCertificates(rootNodes, existingCertificates, strategy);
            
            return rootNodes;
        }

        private Node GetNextNode(string targetCertificate, ICollection<NodeDescription> nodeDescriptions, Strategy strategy, ICollection<string> existingCertificates, Node parentNode = null)
        {
            if (parentNode != null && CheckIsCyclicReference(parentNode))
                return null;

            var nextNodeDescriptions = nodeDescriptions.Where(x => string.Equals(x.Output, targetCertificate, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!nextNodeDescriptions.Any())
                throw new CanNotBeSolvedException($"нет подходящего {nameof(NodeDescription)} для {nameof(targetCertificate)} = {targetCertificate}");
            
            if (nextNodeDescriptions.Count > 1)
            {
                var bestAlternativeNode = GetBestAlternative(nextNodeDescriptions, nodeDescriptions, strategy, existingCertificates);
                bestAlternativeNode.Parent = parentNode;

                return bestAlternativeNode;
            }

            var nextNode = new Node
            {
                Description = nextNodeDescriptions.Single(), 
                Parent = parentNode
            };

            FillSubNodesFor(nextNode, nodeDescriptions, strategy, existingCertificates);
            
            return nextNode;
        }

        private Node GetBestAlternative(ICollection<NodeDescription> alternativesDescriptions, ICollection<NodeDescription> nodeDescriptions, Strategy strategy, ICollection<string> existingCertificates)
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

                FillSubNodesFor(alternative, nodeDescriptions, strategy, existingCertificates);

                alternatives.Add(alternative);
            }

            var comparisonStrategy = _comparisonStrategyProvider.GetComparisonStrategy(strategy);
            alternatives.Sort(comparisonStrategy);
            return alternatives.First(); 
        }

        private void FillSubNodesFor(Node parentNode, ICollection<NodeDescription> nodeDescriptions, Strategy strategy, ICollection<string> existingCertificates)
        {
            if (parentNode.Description.Inputs != null && parentNode.Description.Inputs.Any())
            {
                foreach (var parentInputCertificate in parentNode.Description.Inputs)
                {
                    var subNode = GetNextNode(parentInputCertificate, nodeDescriptions, strategy, existingCertificates, parentNode);

                    if (subNode == null)
                        continue;

                    if (parentNode.Children == null)
                        parentNode.Children = new List<Node>();

                    parentNode.Children.Add(subNode);
                }
            }
        }

        private bool CheckIsCyclicReference(Node node)
        {
            var cyclicReferenceNode = node.SearchParentCyclicReferences();

            if (cyclicReferenceNode == null)
                return false;

            node.Children = null;
            node.NodeType = NodeType.CyclicReference;

            return true;
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

        private void TryResolveCyclicReferences(ICollection<Node> rootNodes, ICollection<string> existingCertificates)
        {
            var cyclicReferences = rootNodes.SelectMany(x => x.GetChildNodesCyclicReferences()).ToList();
            var resolvedReferences = new HashSet<Guid>();
            foreach (var cyclicReference in cyclicReferences)
            {
                var isCyclicReferenceResolved = false;
                var parent = cyclicReference.Parent;
                var signature = cyclicReference.Description.UniqueSignature;
                while (parent != null)
                {
                    if (resolvedReferences.Contains(parent.Id))
                    {
                        isCyclicReferenceResolved = true;
                        break;
                    }

                    if (existingCertificates.Contains(parent.Description.Output))
                    {
                        var stubNode = new Node
                        {
                            Description = new NodeDescription{ Output = parent.Description.Output, Title = "Existing" }, 
                            NodeType = NodeType.StubForExistingCertificate
                        };
                        existingCertificates.Remove(parent.Description.Output);

                        var upperParent = parent.Parent;
                        if (upperParent == null)
                        {
                            rootNodes.Remove(parent);
                            rootNodes.Add(stubNode);
                        }
                        else
                        {
                            upperParent.Children.Remove(parent);
                            upperParent.Children.Add(stubNode);
                        }

                        resolvedReferences.Add(parent.Id);
                        isCyclicReferenceResolved = true;
                        break;
                    }

                    if (!existingCertificates.Contains(parent.Description.Output) && signature.Equals(parent.Description.UniqueSignature))
                        break;

                    parent = parent.Parent;
                }

                if (!isCyclicReferenceResolved)
                    throw new CanNotBeSolvedException($"Найдена циклическая зависимость для справки {cyclicReference.Description.Output} узел {cyclicReference.Description.UniqueSignature}");
            }
        }
    }
}

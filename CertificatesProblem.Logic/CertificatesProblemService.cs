using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Model;
using CertificatesProblem.Model.Exceptions;

namespace CertificatesProblem.Logic
{
    public class CertificatesProblemService : ICertificatesProblemService
    {
        public string Solve(ICollection<NodeDescription> nodeDescriptions, ICollection<string> targetCertificates)
        {
            var targetCertificateToRoodNodeDict = new Dictionary<string, Node>();

            foreach (var targetCertificate in targetCertificates)
            {
                var rootNode = GetNextNode(targetCertificate, nodeDescriptions);

                targetCertificateToRoodNodeDict[targetCertificate] = rootNode;
            }

            var responseBuilder = new StringBuilder();

            foreach (var pair in targetCertificateToRoodNodeDict)
                responseBuilder.AppendLine($"{pair.Key}: {pair.Value.ToFormula()}");

            return responseBuilder.ToString();
        }

        private Node GetNextNode(string targetCertificate, ICollection<NodeDescription> nodeDescriptions, Node parentNode = null)
        {
            var nextNodeDescriptions = nodeDescriptions.Where(x => string.Equals(x.Output, targetCertificate, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!nextNodeDescriptions.Any())
                throw new CanNotBeSolvedException($"нет подходящего {nameof(NodeDescription)} для {nameof(targetCertificate)} = {targetCertificate}");
            
            if (nextNodeDescriptions.Count > 1)
            {
                var bestAlternativeNode = GetBestAlternative(nextNodeDescriptions, nodeDescriptions);
                bestAlternativeNode.Parent = parentNode;

                // todo: проверка на зацикленность назад по графу

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
                    var subNextNode = GetNextNode(nextNodeInputCertificate, nodeDescriptions, nextNode);
                
                    if (nextNode.Children == null)
                        nextNode.Children = new List<Node>();

                    nextNode.Children.Add(subNextNode);
                }
            }
            
            // todo: проверка на зацикленность назад по графу

            return nextNode;
        }

        private Node GetBestAlternative(ICollection<NodeDescription> alternativesDescriptions, ICollection<NodeDescription> nodeDescriptions)
        {
            if (alternativesDescriptions.Select(x => x.Output.ToLower()).Distinct().Count() != 1)
                throw new ArgumentException(nameof(alternativesDescriptions));

            var alternatives = new List<Node>();

            foreach (var alternativesDescription in alternativesDescriptions)
            {
                var alternative = new Node();
                alternative.Description = alternativesDescription;
                alternative.Parent = null;

                if (alternative.Description.Inputs != null && alternative.Description.Inputs.Any())
                {
                    foreach (var alternativeInputCertificate in alternative.Description.Inputs)
                    {
                        var subAlternativeNode = GetNextNode(alternativeInputCertificate, nodeDescriptions, alternative);
                
                        if (alternative.Children == null)
                            alternative.Children = new List<Node>();

                        alternative.Children.Add(subAlternativeNode);
                    }
                }

                alternatives.Add(alternative);
            }

            //alternatives.Sort(); // todo: заменить стратегией сортировки по переходам
            return alternatives.First(); 
        }
    }
}

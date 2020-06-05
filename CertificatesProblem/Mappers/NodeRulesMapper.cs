using System;
using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Dtos.Requests;
using CertificatesProblem.Interfaces.Mappers;
using CertificatesProblem.Model;

namespace CertificatesProblem.Mappers
{
    public class NodeRulesMapper : IMapper<IEnumerable<NodeRules>, IEnumerable<NodeDescription>>
    {
        public IEnumerable<NodeDescription> Map(IEnumerable<NodeRules> nodesRules)
        {
            foreach (var nodeRules in nodesRules)
            {
                foreach (var nodeRuleInOut in nodeRules.InOuts)
                {
                    var node = new NodeDescription
                    {
                        Title = nodeRules.NodeId,
                        Inputs = nodeRuleInOut.RequiredInputs?.Select(x => x.CertificateId).ToList(),
                        Output = nodeRuleInOut.Output.CertificateId,
                        TimeCost = new TimeSpan(nodeRuleInOut.TimeCost.Days, nodeRuleInOut.TimeCost.Hours, nodeRuleInOut.TimeCost.Minutes, 0),
                        MoneyCost = nodeRuleInOut.MoneyCost
                    };

                    yield return node;
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using CertificatesProblem.Dtos.Requests;
using CertificatesProblem.Dtos.Results;
using CertificatesProblem.Interfaces;
using CertificatesProblem.Interfaces.Mappers;
using CertificatesProblem.Model;
using CertificatesProblem.Model.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CertificatesProblem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CertificatesProblemController : ControllerBase
    {
        private readonly IMapper<IEnumerable<NodeRules>, IEnumerable<NodeDescription>> _nodeRulesMapper;
        private readonly IMapper<IEnumerable<Certificate>, IEnumerable<string>> _targetCertificatesMapper;
        private readonly IMapper<StrategyRequest, Strategy> _strategyMapper;
        private readonly IMapper<ICollection<Node>, CertificatesProblemSolution> _solutionMapper;
        

        private readonly ICertificatesProblemService _certificatesProblemService;

        public CertificatesProblemController(
            IMapper<IEnumerable<NodeRules>, IEnumerable<NodeDescription>> nodeRulesMapper, 
            IMapper<IEnumerable<Certificate>, IEnumerable<string>> targetCertificatesMapper, 
            IMapper<StrategyRequest, Strategy> strategyMapper,
            IMapper<ICollection<Node>, CertificatesProblemSolution> solutionMapper,
            ICertificatesProblemService certificatesProblemService)
        {
            _nodeRulesMapper = nodeRulesMapper;
            _targetCertificatesMapper = targetCertificatesMapper;
            _certificatesProblemService = certificatesProblemService;
            _strategyMapper = strategyMapper;
            _solutionMapper = solutionMapper;
        }

        [HttpPost("solve")]
        public ActionResult<CertificatesProblemSolution> Solve([FromBody] CertificatesProblemRequest request)
        {
            var nodeDescriptions = _nodeRulesMapper.Map(request.NodesRules).ToList();
            var targets = _targetCertificatesMapper.Map(request.TargetCertificates).ToList();
            var strategy = _strategyMapper.Map(request.Strategy);

            try
            {
                var rootNodes = _certificatesProblemService.Solve(nodeDescriptions, targets, strategy);

                var solution = _solutionMapper.Map(rootNodes);

                return new JsonResult(solution);
            }
            catch (CanNotBeSolvedException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
using SAM.NUGET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SAM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionController : ControllerBase
    {
        private readonly IAgentServices _agentServices;

        public CommissionController(IAgentServices agentServices)
        {
            _agentServices = agentServices;
        }

        [HttpGet]
        [Route("orc/scenarios")]
        public IActionResult GetOrcScenarios()
        {
            try
            {
                var model = _agentServices.GetOrcScenarios();
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "OrcScenarioRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("orc/scenarios/{scenarioId}")]
        public IActionResult GetOrcScenarioDetails([FromRoute] string scenarioId)
        {
            try
            {
                var model = _agentServices.GetOrcScenarioDetails(scenarioId);
                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "OrcScenarioDetailsRetrievalError"
                });
            }
        }
    }
}

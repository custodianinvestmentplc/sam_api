using SAM.NUGET.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SAM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesTeamController : ControllerBase
    {
        private readonly IAgentServices _agentServices;

        public SalesTeamController(IAgentServices agentServices)
        {
            _agentServices = agentServices;
        }

        [HttpGet]
        public IActionResult GetAllAgents()
        {
            try
            {
                var model = _agentServices.GetAllAgents();
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SalesTeamRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("agents/{id}")]
        public IActionResult GetAgentDetails([FromRoute] string id)
        {
            try
            {
                var model = _agentServices.GetAgent(id);
                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SalesAgentRetrievalError"
                });
            }
        }
    }
}

using SAM.NUGET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Linq;
using SAM.NUGET.Domain.Dtos;
using System.Collections.Generic;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentServices _agentServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AgentsController(IAgentServices agentServices)
        {
            _agentServices = agentServices;
        }

        [HttpGet]
        [Route("find")]
        public IActionResult FindAgentByName([FromQuery] string searchTerm, string Ref)
        {
            try
            {
                //var model = new List<SalesTeamMemberDto>()
                //{
                //    new SalesTeamMemberDto
                //    {
                //        AgentSystemCode = "test1",
                //        BusinessCode = "test",
                //         FullName = "AgentTest1",
                //         SalesLevel = "test",
                //    },
                //    new SalesTeamMemberDto
                //    {
                //        AgentSystemCode = "test2",
                //        BusinessCode = "test",
                //         FullName = "AgentTest2",
                //         SalesLevel = "test",
                //    },
                //};

                var model = _agentServices.FindAgentByName(searchTerm, Ref);

                return StatusCode(200, model);

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcFindAgentError"
                });
            }
        }
    }
}

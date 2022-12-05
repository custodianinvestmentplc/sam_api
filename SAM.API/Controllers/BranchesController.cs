using SAM.NUGET.Domain.Dtos;
using SAM.NUGET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BranchesController(ICPCHubServices cpcHubServices)
        {
            _cpcServices = cpcHubServices;
        }

        [HttpGet]
        public IActionResult GetAllBranches()
        {
            try
            {
                var model = _cpcServices.GetCpcBranches();
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcBranchesRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("branchesandroles")]
        public IActionResult GetBranchesAndRoles()
        {
            try
            {
                var branches = _cpcServices.GetCpcBranches().ToArray();
                var roles = _cpcServices.GetCpcRoles().ToArray();

                var model = new CPCBranchAndRolesDto()
                {
                    Branches = branches,
                    Roles = roles
                };

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "CpcBranchesAndRolesRetrievalError"
                });
            }
        }

        [HttpGet]
        [Route("states")]
        public IActionResult GetAllStates()
        {
            try
            {
                var model = _cpcServices.GetStatesInNigeria();

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                return StatusCode(500, new
                {
                    ErrorDescription = $"CpcStateRetrievalError: { ex.Message }",
                    ExceptionType = "DataRetrievalError"
                });
            }
        }
    }
}

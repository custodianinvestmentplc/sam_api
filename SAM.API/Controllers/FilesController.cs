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
    public class FilesController : ControllerBase
    {
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FilesController(ICPCHubServices cpcHubServices)
        {
            _cpcServices = cpcHubServices;
        }

        [HttpGet]
        [Route("cpc")]
        public IActionResult GetAllCpcFiless()
        {
            try
            {
                var model = _cpcServices.GetCpcFiles();
              
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = $"GetCpcFilesError: {ex.Message}",
                    ExceptionType = "DataRetrievalError"
                });
            }
        }
    }
}

using SAM.NUGET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportingController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("modules/reports")]
        public IActionResult GetReportList([FromQuery] int id, string useremail)
        {
            try
            {
                var model = _reportService.GetReportByModuleId(id, useremail);
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "ReportListRetrievalError"
                });
            }
        }
    }
}

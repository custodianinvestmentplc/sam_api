using SAM.NUGET.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupSynergyServices _groupService;

        public GroupController(IGroupSynergyServices groupservice)
        {
            _groupService = groupservice;
        }

        [HttpGet]
        [Route("customers/search")]
        public IActionResult Search([FromQuery] string searchTerm)
        {
            try
            {
                var model = _groupService.SearchGroupCustomerDatabase(searchTerm);
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GroupSearchError"
                });
            }
        }

        [HttpGet]
        [Route("customers/{fuzzykey}")]
        public IActionResult SearchByFuzzyKey([FromRoute] int fuzzykey)
        {
            try
            {
                var model = _groupService.GetCustomerSearchDetailsDtos(fuzzykey);
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GroupSearchDetailError"
                });
            }
        }

        [HttpGet]
        [Route("customers")]
        public IActionResult FetchAllCustomers()
        {
            try
            {
                var model = _groupService.FetchAllCustomers();
                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "AllCustomersRetrievalError"
                });
            }
        }
    }
}

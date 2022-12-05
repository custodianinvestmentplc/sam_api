using SAM.NUGET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using SAM.NUGET.Payloads.Cts;
using System.Linq;
using System.Web;
using SAM.NUGET.Domain.RequestModels;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtsController : ControllerBase
    {
        private readonly ICtsService _ctsService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CtsController(ICtsService ctsService)
        {
            _ctsService = ctsService;
        }

        [HttpGet]
        [Route("tickets/new")]
        public IActionResult FetchNewTickets()
        {
            try
            {
                var model = _ctsService.FetchNewCtsTickets();

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetNewTicketsError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/{ticketNumber}/activity-log")]
        public IActionResult TicketActivityLog([FromRoute] string ticketNumber)
        {
            try
            {
                var model = _ctsService.GetTicketActivityLog(ticketNumber);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetTicketActivityLogError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/search")]
        public IActionResult FetchNewTickets([FromQuery] string searchTerm)
        {
            try
            {
                var sTerm = HttpUtility.UrlDecode(searchTerm);
                var model = _ctsService.FindTicketByTitle(sTerm);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetNewTicketsError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/{ticketNumber}")]
        public IActionResult GetTicketDetailsById([FromRoute] string ticketNumber)
        {
            try
            {
                var model = _ctsService.GetTicketByTicketNumber(ticketNumber);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetTicketError"
                });
            }
        }

        [HttpGet]
        [Route("tickets/{ticketNumber}/categorization")]
        public IActionResult GetTicketCategorization([FromRoute] string ticketNumber)
        {
            try
            {
                var model = _ctsService.GetTicketCategorization(ticketNumber);

                return StatusCode(200, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "GetTicketCategorizationError"
                });
            }
        }

        [HttpGet]
        [Route("companies")]
        public IActionResult FetchAllCompanies()
        {
            try
            {
                var model = _ctsService.FetchAllCompanies();

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetCompaniesError"
                });
            }
        }

        [HttpGet]
        [Route("service-types")]
        public IActionResult FetchServiceTypes()
        {
            try
            {
                var model = _ctsService.FetchServiceTypes();

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeError"
                });
            }
        }

        [HttpGet]
        [Route("service-types/{serviceTypeId}")]
        public IActionResult GetServiceType([FromRoute] int serviceTypeId)
        {
            try
            {
                var model = _ctsService.GetServiceTypeDetails(serviceTypeId);

                if (model != null)
                {
                    return StatusCode(200, model);
                }

                return StatusCode(404, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeError"
                });
            }
        }

        [HttpGet]
        [Route("service-types/{serviceTypeId}/categories")]
        public IActionResult FetchCategoriesByServiceTypeId([FromRoute] int serviceTypeId)
        {
            try
            {
                var model = _ctsService.FetchCategoryByServiceTypeId(serviceTypeId);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("categories/{categoryId}/sub-categories")]
        public IActionResult FetchSubcategoriesByCategoryId([FromRoute] int categoryId)
        {
            try
            {
                var model = _ctsService.FetchSubCategoryByCategoryId(categoryId);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("categories/{categoryId}")]
        public IActionResult GetCategoryDetail([FromRoute] int categoryId)
        {
            try
            {
                var model = _ctsService.GetCategoryDetails(categoryId);

                if (model != null)
                {
                    return StatusCode(200, model);
                }

                return StatusCode(404, new
                {
                    ErrorDescription = $"Could not find the Category with Id { categoryId } in the database",
                    ExceptionType = "InvalidCategoryId"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetCategoryError"
                });
            }
        }

        [HttpGet]
        [Route("sub-categories/{subcategoryId}/subcategory-items")]
        public IActionResult FetchSubcategoryItemIdBySubcategoryId([FromRoute] int subcategoryId)
        {
            try
            {
                var model = _ctsService.FetchSubCategoryItemBySubCategoryId(subcategoryId);

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("sub-categories/{subcategoryId}")]
        public IActionResult GetSubcategoryDetail([FromRoute] int subcategoryId)
        {
            try
            {
                var model = _ctsService.GetSubCategoryDetails(subcategoryId);

                if (model != null)
                {
                    return StatusCode(200, model);
                }

                return StatusCode(404, model);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    errorDescription = ex.Message,
                    exceptionType = "GetServiceTypeCategoriesError"
                });
            }
        }

        [HttpGet]
        [Route("GetAllTechnicians")]
        public IActionResult GetAllTechnicians()
        {
            try
            {
                var model = _ctsService.FetchAllTechnicians();

                return StatusCode(200, model.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "FetchTechniciansError"
                });
            }
        }

        [HttpPost]
        [Route("assign-new-ticket")]
        public IActionResult AllocateAndAssignNewTicket([FromBody] AssignNewTicket payload)
        {
            try
            {
                var model = _ctsService.FetchAllTechnicians();

                var caller = model.FirstOrDefault(x => x.UserEmail == payload.UserEmail);
                var callerId = caller == null ? 13 : caller.UserId;

                var ticket = _ctsService.GetTicketByTicketNumber(payload.TicketNumber);

                var req = new AssignTicketRequest
                {
                    CategoryId = payload.CategoryId,
                    CompanyId = payload.CompanyId,
                    SeverityId = payload.SeverityId,
                    SubcategoryId = payload.SubcategoryId,
                    SubcategoryItemId = payload.SubcategoryItemId,
                    TicketNumber = payload.TicketNumber,
                    TicketRequestId = ticket.RequestId,
                    UserEmail = payload.UserEmail,
                    UserId = callerId,
                    TechnicianId = payload.TechnicianId
                };

                _ctsService.AssignNewTicket(req);

                return StatusCode(201, new 
                {
                    ErrorMessage = "Operation Successful"
                });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(501, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "FetchTechniciansError"
                });
            }
        }
    }
}

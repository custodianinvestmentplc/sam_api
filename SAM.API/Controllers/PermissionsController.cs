
using SAM.NUGET.Domain.Dtos;
using SAM.NUGET.Domain.Options;
using SAM.NUGET.Models;
using SAM.NUGET.Services;
using SAM.NUGET.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;
        private readonly ICPCHubServices _cpcServices;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PermissionsController(LoginConfig config, IUserServices userServices, IAuthProvider authprovider, ICPCHubServices cpcHubServices)
        {
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
            _cpcServices = cpcHubServices;
        }


        [HttpGet]
        public IActionResult Authorization([FromQuery] string useremail, string form)
        {
            try
            {
                var permissionOptions = new PermissionOptions()
                {
                    Form = form,
                    UserEmail = useremail
                };

                var permissions = _cpcServices.GetPermissions(permissionOptions);

                //var permissions = new List<string>
                //{
                //    "can_view",
                //    "can_edit",
                //    "can_submit"
                //};

                return StatusCode(200, permissions);

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "PermissionsError"
                });

            }
        }
    }
}

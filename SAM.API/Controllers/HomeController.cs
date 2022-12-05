using SAM.NUGET.Domain.Dtos;
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
    public class HomeController : ControllerBase
    {
        private readonly LoginConfig _config;
        private readonly IUserServices _userServices;
        private readonly IAuthProvider _authProvider;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController(LoginConfig config, IUserServices userServices, IAuthProvider authprovider)
        {
            _config = config;
            _userServices = userServices;
            _authProvider = authprovider;
        }

        [HttpPost]
        public async Task<IActionResult> Callback(string code, string state, string error)
        {
            var indexVM = new IndexViewModel();

            try
            {
                var token = _authProvider.AcquireAdToken(code);
                var adUser = _authProvider.GetLoggedInUser(token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, adUser.UserPrincipalName)
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, props);

                return RedirectToAction("GetUser", "User");

            }
            catch (Exception ex)
            {
                indexVM.ErrorTitle = "Unable to signin user";
                indexVM.ExceptionType = "Authentication Error";
                indexVM.ErrorDescription = ex.Message;

                log.Error(DateTime.Now.ToString(), ex);

                return StatusCode(500, indexVM);
            }
        }
    }
}

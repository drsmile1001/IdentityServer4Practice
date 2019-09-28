using IdentityServer4.Services;
using IdentityServer4AsApi.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4AsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticateController(
            IIdentityServerInteractionService interaction,
            IWebHostEnvironment environment,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _interaction = interaction;
            _environment = environment;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ReturnUrl { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)

        {
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password,isPersistent:false, lockoutOnFailure: false);

            if (result.Succeeded  && context != null)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                await HttpContext.SignInAsync(user.Id, user.UserName);
                return Ok(new { RedirectUrl = request.ReturnUrl, IsOk = true });
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            bool showSignoutPrompt = true;

            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                showSignoutPrompt = false;
            }

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
            }

            // no external signout supported for now (see \Quickstart\Account\AccountController.cs TriggerExternalSignout)
            return Ok(new
            {
                showSignoutPrompt,
                ClientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName,
                context?.PostLogoutRedirectUri,
                context?.SignOutIFrameUrl,
                logoutId
            });
        }

        [HttpGet]
        [Route("Error")]
        public async Task<IActionResult> Error(string errorId)
        {
            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);

            if (message != null)
            {
                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return Ok(message);
        }
    }
}
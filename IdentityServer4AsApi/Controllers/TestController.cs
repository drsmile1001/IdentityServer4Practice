using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4;

namespace IdentityServer4AsApi.Controllers
{
    
    [ApiController]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public class TestController : ControllerBase
    {
        [Route("api/test/get")]
        public IActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToArray();
            return Ok(new { message = "Hello API", claims });
        }
    }
}
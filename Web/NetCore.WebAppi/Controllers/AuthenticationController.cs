using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.WebAppi.Identity.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCore.WebAppi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        ///
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mediator"></param>
        public AuthenticationController(IMediator mediator)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Hello World
        /// </summary>
        /// <param name="request">Param 1</param>
        /// <returns>Response</returns>
        /// <remarks>Hello</remarks>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserAuthenticateCommand.Request request)
        {
            ObjectResult result = null;
            try
            {
                UserAuthenticateCommand.Response response = await Mediator.Send(request);

                result = Ok(response.Token);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        [HttpGet("test")]
        [Authorize(Roles = "Admin")]
        public bool Test()
        {
            string name = User.Identity.Name;

            IEnumerable<Claim> Claims = User.Claims.Where(x => x.Type == ClaimTypes.Role);

            foreach (Claim claim in Claims)
            {
                Trace.WriteLine(claim.Value);
            }

            return true;
        }
    }
}
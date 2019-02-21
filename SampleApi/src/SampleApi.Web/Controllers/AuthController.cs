using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleApi.Web.Helpers;
using SampleApi.Web.Infrastructure;
using SampleApi.Web.Models;
using SampleApi.Web.Services.User;

namespace SampleApi.Web.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SignInModel model)
        {
            try
            {
                User user;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, model.UserName),
                    new Claim("name", model.UserName),
                    //new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var isValid = await _userService.ValidateCredentials(model.UserName, model.Password, out user);
                if (isValid)
                {
                    var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(InMemoryConfig.SecretKey));

                    //https://stackoverflow.com/questions/49875167/jwt-error-idx10634-unable-to-create-the-signatureprovider-c-sharp
                    var creds = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature);

                    var token = new JwtSecurityToken(InMemoryConfig.Issuer, InMemoryConfig.Audience, claims,
                        expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: creds);

                    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(Envelop.Ok(tokenValue));
                }
                return BadRequest(Envelop.Error("User name or password is incorrect!"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

    }
}

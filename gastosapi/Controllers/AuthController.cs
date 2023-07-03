using gastosapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gastosapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GastosappContext _context;

        public AuthController(GastosappContext context)
        {
            _context = context;   
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login user)
        {
            var authUser = false;
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            authUser = _context.Users.Any(u => u.Username == user.UserName && u.Password == user.Password);
            if(authUser)
            {
                var loggedUser = _context.Users.Single(u => u.Username == user.UserName && u.Password == user.Password);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("IdUser", loggedUser.IdUser.ToString()),
                    new Claim("UserName", loggedUser.Username.ToString())

                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7176",
                    audience: "https://localhost:7176",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new AuthenticatedResponse { Token = tokenString });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'GastosappContext.Users'  is null.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUser", new { id = user.IdUser }, user);

            // generate jwt token
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7176",
                audience: "https://localhost:7176",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new AuthenticatedResponse { Token = tokenString });
        }

    }
}

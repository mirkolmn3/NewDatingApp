using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewDatingApp.API.Data;
using NewDatingApp.API.Dtos;
using NewDatingApp.API.Models;

namespace NewDatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region PROPERTIES
        private readonly IAuthRepository repo;
        #endregion PROPERTIES
        private readonly IConfiguration config;

        #region CONSTRUCTOR
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;
        }
        #endregion CONSTRUCTOR

        #region CONTROLLERS
        [HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request
            // if(!ModelState.IsValid)
            //     return BadRequest(ModelState);
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

            if (await repo.UserExists(userForRegisterDto.UserName))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };

            var createdUser = await this.repo.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201);
            // return CreatedAtRoute() 
            // 
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await this.repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        #endregion CONTROLLERS
    }
}
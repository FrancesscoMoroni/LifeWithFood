using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
    {

        private readonly LifeWithFoodDbContext _dbcontext;
        private readonly IConfiguration _configuration;

        public UserAuthenticationController(LifeWithFoodDbContext dbcontext, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("userregister")]
        public async Task<ActionResult<AuthorizationDto>> Register(UserDto userDto)
        {
            var authorizationDto = new AuthorizationDto();

            if (AutherizationRegister(userDto))
            {
                authorizationDto.Error = CreateUser(userDto);
                authorizationDto.Jwt = CreateToken(userDto);
            }
            else
            {
                authorizationDto.Error = "Błąd rejestracji";
            }

            return Ok(authorizationDto);
        }

        [HttpPost]
        [Route("userlogin")]
        public async Task<ActionResult<AuthorizationDto>> Login(UserDto userDto)
        {
            var authorizationDto = new AuthorizationDto();

            if (AutherizationLogin(userDto))
            {
                authorizationDto.Jwt = CreateToken(userDto);
            } else
            {
                authorizationDto.Error = "Błąd autoryzacji";
            }

            return Ok(authorizationDto);
        }

        private string CreateUser(UserDto userDto)
        {
            var newUser = new User();

            newUser.Login = userDto.Login;
            newUser.Password = userDto.Password;
            newUser.Name = "";
            newUser.Role = 1;
            newUser.CreateDate = DateTime.Now;

            try
            {
                _dbcontext.Add<User>(newUser);
                _dbcontext.SaveChanges();
            } catch (Exception ex)
            {
                return "Błąd przy tworzeniu użytkownika";
            }

            return "";
        }

        private bool AutherizationRegister(UserDto userDto)
        {
            try
            {
                return _dbcontext.Users.Where(e => e.Login == userDto.Login).IsNullOrEmpty();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool AutherizationLogin(UserDto userDto)
        {
            try
            {
                return !_dbcontext.Users.Where(e => e.Login == userDto.Login && e.Password == userDto.Password).IsNullOrEmpty();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string CreateToken(UserDto userDto)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, userDto.Login)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}

using LifeWithFood.Data;
using LifeWithFood.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
    {

        private readonly LifeWithFoodDbContext _dbcontext;


        public UserAuthenticationController(LifeWithFoodDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("userlogin")]
        public async Task<ActionResult<bool>> Login(UserDto userDto)
        {
            return Ok(Autherization(userDto));
        }

        private bool Autherization(UserDto userDto)
        {
            try
            {
                return !_dbcontext.Users.Where(e => e.Login == userDto.login && e.Password == userDto.password).IsNullOrEmpty();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

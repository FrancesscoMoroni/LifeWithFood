using Microsoft.AspNetCore.Mvc;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hejka naklejka";
        }
    }
}

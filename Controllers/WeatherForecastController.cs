using LifeWithFood.Data;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly LifeWithFoodDbContext _dbcontext;

        public WeatherForecastController(LifeWithFoodDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public List<Tag> GetTags()
        {
            try {
                return _dbcontext.Tags.ToList();
            } catch (Exception ex) {
                return null;
            }
        }
    }
}
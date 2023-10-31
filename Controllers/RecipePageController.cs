using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipePageController : ControllerBase
    {
        private readonly LifeWithFoodDbContext _dbcontext;

        public RecipePageController(LifeWithFoodDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("getrecipe")]
        public async Task<ActionResult<Recipe>> GetPage(RecipeIdDto id)
        {
            Recipe recipe = null;

            try
            {
                recipe = _dbcontext.Recipes.Where(e => e.IdRecipe == id.Id).FirstOrDefault();
            } catch
            {
                return Ok(recipe);
            }

            return Ok(recipe);
        }

    }
}
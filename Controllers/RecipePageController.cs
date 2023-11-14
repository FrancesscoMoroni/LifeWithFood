using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        [Authorize]
        [Route("iffavoriterecipe")]
        public async Task<ActionResult<bool>> IfFavoriteRecipe(RecipeIdDto recipeId)
        {
            String userName = HttpContext.User.Identity.Name;
            try
            {
                Recipe favoriteRecipe = _dbcontext.Recipes
                    .Where(r => r.IdRecipe == recipeId.Id)
                    .FirstOrDefault();
                User currentUser = _dbcontext.Users
                    .Where(u => u.Login == userName)
                    .Include(u => u.RecipesIdRecipes)
                    .FirstOrDefault();

                if (currentUser.RecipesIdRecipes.Contains(favoriteRecipe))
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }

            }
            catch
            {
                return Ok("Błąd sprawdzania ulubionych przepisów");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("menagefovoriterecipe")]
        public async Task<ActionResult<string>> MenageFovoriteRecipe(RecipeIdDto recipeId)
        {
            String userName = HttpContext.User.Identity.Name;
            try
            {
                Recipe favoriteRecipe = _dbcontext.Recipes
                    .Where(r => r.IdRecipe == recipeId.Id)
                    .FirstOrDefault();

                User currentUser = _dbcontext.Users
                    .Where(u => u.Login == userName)
                    .Include(u => u.RecipesIdRecipes)
                    .FirstOrDefault();

                if (currentUser.RecipesIdRecipes.Contains(favoriteRecipe))
                {
                    currentUser.RecipesIdRecipes.Remove(favoriteRecipe);
                }
                else
                {
                    currentUser.RecipesIdRecipes.Add(favoriteRecipe);
                }

                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania do ulubionych");
            }

            return Ok("");

        }

    }
}
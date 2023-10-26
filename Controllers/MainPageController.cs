using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainPageController : ControllerBase
    {
        private readonly LifeWithFoodDbContext _dbcontext;

        public MainPageController(LifeWithFoodDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("getpage")]
        public async Task<ActionResult<List<Recipe>>> GetPage(PaginatorDto paginatorDto)
        {
            List<RecipeDto> pageList = new List<RecipeDto>();

            try
            {
                pageList = _dbcontext.Recipes.Select( s => new RecipeDto { Name = s.Name, Description = s.Description, Id = s.IdRecipe}).Skip(paginatorDto.PageIndex * paginatorDto.PageSize).Take(paginatorDto.PageSize).ToList<RecipeDto>();
            } catch
            {
                return Ok(pageList);
            }

            return Ok(pageList);
        }

        [HttpGet]
        [Route("getnumberofrecipes")]
        public async Task<ActionResult<int>> GetNumberOfRecipes()
        {
            int numberOfRecipes = _dbcontext.Recipes.Count();
            return Ok(numberOfRecipes);
        }

    }
}
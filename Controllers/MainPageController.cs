using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

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
        public async Task<ActionResult<List<RecipeCardDto>>> GetPage(PaginatorDto paginatorDto)
        {
            List<RecipeCardDto> pageList = new List<RecipeCardDto>();
            var query = _dbcontext.Recipes.AsQueryable();

            try
            {
                query = _dbcontext.Recipes
                           .Include(r => r.TagsIdTags)
                           .Include(r => r.ListsOfIngredients)
                           .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                           .Include(r => r.Ratings)
                           .Where(r => r.Name.StartsWith(paginatorDto.name));

                if (!paginatorDto.filtr.IsNullOrEmpty())
                {
                    query = query.Where(r => r.TagsIdTags.Any(t => paginatorDto.filtr.Contains(t.IdTag)));
                }

                var queryRecipeCard = query.Select(s => new RecipeCardDto
                {
                    Name = s.Name,
                    Description = s.Description,
                    Id = s.IdRecipe,
                    PrepTime = s.PrepTime,
                    CreateDate = s.CreateDate,
                    Tags = s.TagsIdTags.Select(t => new TagDto
                    {
                        Name = t.Name,
                        Priority = t.Priority
                    }).OrderBy(t => t.Priority).ToList(),
                    Score = s.Ratings.Count != 0 ? s.Ratings.Sum(r => r.Score) / s.Ratings.Count : 0,
                });

                switch (paginatorDto.sort)
                {
                    case 1:
                        queryRecipeCard = queryRecipeCard.OrderBy(r => r.Name);
                        break;
                    case 2:
                        queryRecipeCard = queryRecipeCard.OrderByDescending(r => r.Name);
                        break;
                    case 3:
                        queryRecipeCard = queryRecipeCard.OrderBy(r => r.PrepTime);
                        break;
                    case 4:
                        queryRecipeCard = queryRecipeCard.OrderByDescending(r => r.PrepTime);
                        break;
                    case 5:
                        queryRecipeCard = queryRecipeCard.OrderBy(r => r.Score);
                        break;
                    case 6:
                        queryRecipeCard = queryRecipeCard.OrderByDescending(r => r.Score);
                        break;
                    case 7:
                        queryRecipeCard = queryRecipeCard.OrderBy(r => r.CreateDate);
                        break;
                    case 8:
                        queryRecipeCard = queryRecipeCard.OrderByDescending(r => r.CreateDate);
                        break;
                    default:
                        queryRecipeCard = queryRecipeCard.OrderBy(r => r.Id);
                        break;
                }
                pageList = queryRecipeCard
                    .Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                    .Take(paginatorDto.PageSize)
                    .ToList();
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
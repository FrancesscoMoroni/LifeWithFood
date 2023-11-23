using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Formats.Asn1.AsnWriter;

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
        public async Task<ActionResult<Recipe>> GetPage(IdDto id)
        {
            RecipePageDto recipe = null;

            try
            {
                recipe = _dbcontext.Recipes.Where(e => e.IdRecipe == id.Id)
                    .Include(r => r.UsersIdUserNavigation)
                    .Include(r => r.TagsIdTags)
                    .Include(r => r.Ratings)
                    .Include(r => r.ListsOfIngredients)
                    .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                    .Select(r => new RecipePageDto
                    {
                        IdRecipe = r.IdRecipe,
                        Name = r.Name,
                        Description = r.Description,
                        Instruction = r.Instruction,
                        PrepTime = r.PrepTime,
                        CreateDate = r.CreateDate,
                        EditDate = r.EditDate,
                        CreatorName = r.UsersIdUserNavigation.Login,
                        tags = r.TagsIdTags
                            .Select(t => new TagDto
                            {
                                Name = t.Name,
                                Priority = t.Priority
                            })
                            .ToList(),
                        ingredients = r.ListsOfIngredients
                            .Select(l => new RecipeIngredientDto
                            {
                                Name = l.GroceriesIdFoodItemNavigation.Name,
                                Quantity = l.Quanity,
                                Unit = l.GroceriesIdFoodItemNavigation.Unit
                            })
                        .ToList(),
                        ratings = r.Ratings
                            .Select(r => new RatingDto
                            {
                                IdRating = r.IdRating,
                                Comment = r.Comment,
                                Score = r.Score,
                                UserName = r.UsersIdUserNavigation.Login,
                                Date = r.Date
                            })
                            .ToList(),
                        finalScore = r.Ratings.Sum(r => r.Score),
                    })
                    .FirstOrDefault();
            } catch
            {
                return Ok(recipe);
            }

            return Ok(recipe);
        }

        [HttpPost]
        [Authorize]
        [Route("iffavoriterecipe")]
        public async Task<ActionResult<bool>> IfFavoriteRecipe(IdDto idRecipe)
        {
            String userName = HttpContext.User.Identity.Name;
            try
            {
                Recipe favoriteRecipe = _dbcontext.Recipes
                    .Where(r => r.IdRecipe == idRecipe.Id)
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
        public async Task<ActionResult<string>> MenageFovoriteRecipe(IdDto idRecipe)
        {
            String userName = HttpContext.User.Identity.Name;
            try
            {
                Recipe favoriteRecipe = _dbcontext.Recipes
                    .Where(r => r.IdRecipe == idRecipe.Id)
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

        [HttpPost]
        [Authorize]
        [Route("addnewrating")]
        public async Task<ActionResult<string>> AddNewRating(RatingDto ratingDto)
        {
            String userName = HttpContext.User.Identity.Name;

            Rating newRating = new Rating();
            newRating.Comment = ratingDto.Comment;
            newRating.Score = ratingDto.Score;
            newRating.Date = DateTime.Now;
            
            try
            {
                newRating.UsersIdUserNavigation = _dbcontext.Users.Where(u => u.Login == userName).FirstOrDefault();
                newRating.RecipesIdRecipeNavigation = _dbcontext.Recipes.Where(r => r.IdRecipe == ratingDto.IdRecipe).FirstOrDefault();

                _dbcontext.Ratings.Add(newRating);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd podczas dodawania komentarza");
            }

            return Ok("");
        }

        [HttpPost]
        [Authorize]
        [Route("deleterating")]
        public async Task<ActionResult<string>> DeleteRating(RatingDto ratingDto)
        {

            try
            {
                Rating deleteRating = _dbcontext.Ratings
                    .Where(r => r.IdRating == ratingDto.IdRecipe)
                    .FirstOrDefault();

                _dbcontext.Ratings.Remove(deleteRating);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd podczas usuwania komentarza");
            }

            return Ok("");
        }

        [HttpPost]
        [Authorize]
        [Route("getratings")]
        public async Task<ActionResult<List<RatingDto>>> GetRatings(RatingDto ratingDto)
        {

            List<RatingDto> recipeRatings = new List<RatingDto>();

            try
            {
                recipeRatings = _dbcontext.Ratings
                    .Include(r => r.UsersIdUserNavigation)
                    .Where(r => r.RecipesIdRecipe == ratingDto.IdRecipe)
                    .Select(r => new RatingDto {
                        IdRating = r.IdRating,
                        IdRecipe = r.RecipesIdRecipe,
                        Comment = r.Comment,
                        Date = r.Date,
                        Score = r.Score,
                        UserName = r.UsersIdUserNavigation.Login
                    })
                    .ToList();

            }
            catch
            {
                return Ok(recipeRatings);
            }

            return Ok(recipeRatings);
        }

        [HttpPost]
        [Authorize]
        [Route("checkownedingredients")]
        public async Task<ActionResult<List<RecipeIngredientDto>>> CheckOwnedIngredients(IdDto idRecipe)
        {
            String userName = HttpContext.User.Identity.Name;

            List<RecipeIngredientDto> ingredientsToBuy = new List<RecipeIngredientDto>();
            List<RecipeIngredientDto> recipeIngredients = new List<RecipeIngredientDto>();
            List<RecipeIngredientDto> userIngredients = new List<RecipeIngredientDto>();

            try
            {
                recipeIngredients = _dbcontext.Recipes
                    .Include(r => r.ListsOfIngredients)
                    .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                    .Where(r => r.IdRecipe == idRecipe.Id)
                    .FirstOrDefault()
                    .ListsOfIngredients
                    .Select(i => new RecipeIngredientDto
                    {
                        Name = i.GroceriesIdFoodItemNavigation.Name,
                        Quantity = i.Quanity,
                        Unit = i.GroceriesIdFoodItemNavigation.Unit
                    })
                    .ToList();

                userIngredients = _dbcontext.Users
                    .Where(u => u.Login == userName)
                    .Include(u => u.OwnedGroceries)
                    .ThenInclude(o => o.GroceriesIdFoodItemNavigation)
                    .FirstOrDefault()
                    .OwnedGroceries
                    .GroupBy(o => o.GroceriesIdFoodItemNavigation.Name)
                    .Select(o => new RecipeIngredientDto
                    {
                        Name = o.Key,
                        Quantity = o.Sum(o => o.Quanity),
                        Unit = o.Where(t => t.GroceriesIdFoodItemNavigation.Name == o.Key).First().GroceriesIdFoodItemNavigation.Unit
                    })
                    .ToList();
            }
            catch
            {
                return Ok("Błąd sprawdzania ulubionych przepisów");
            }

            recipeIngredients.ForEach(r =>{
                if (userIngredients.Where(u => u.Name == r.Name).IsNullOrEmpty())
                {
                    ingredientsToBuy.Add(r);
                } else
                {
                    var diffrence = r.Quantity - userIngredients.Where(u => u.Name == r.Name).First().Quantity;

                    if (diffrence > 0)
                    {
                        ingredientsToBuy.Add( new RecipeIngredientDto{
                            Name = r.Name,
                            Quantity = diffrence,
                            Unit = r.Unit
                        });
                    }
                }
            });

            return Ok(ingredientsToBuy);
        }

    }
}
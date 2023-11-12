using Azure;
using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminDataController : ControllerBase
    {

        private readonly LifeWithFoodDbContext _dbcontext;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters tokenValidationParameters;

        public AdminDataController(LifeWithFoodDbContext dbcontext, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            this._configuration = configuration;

            tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        // Tags

        [HttpPost]
        [Route("gettags")]
        [Authorize]
        public async Task<ActionResult<List<Tag>>> GetTags(PaginatorDto paginatorDto)
        {
            List<Tag> pageList = new List<Tag>();
            var query = _dbcontext.Tags.AsQueryable();

            try
            {
                query = query.Where(t => t.Name.StartsWith(paginatorDto.name));

                switch (paginatorDto.sort)
                {
                    case 1:
                        query = query.OrderBy(t => t.Name);
                        break;
                    case 2:
                        query = query.OrderByDescending(t => t.Name);
                        break;
                    case 3:
                        query = query.OrderBy(t => t.Priority);
                        break;
                    case 4:
                        query = query.OrderByDescending(t => t.Priority);
                        break;
                    default:
                        query = query.OrderBy(t => t.IdTag);
                        break;
                }
                pageList = query.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                            .Take(paginatorDto.PageSize).ToList<Tag>();
            }
            catch
            {
                return Ok(pageList);
            }

            return Ok(pageList);
        }

        [HttpPost]
        [Route("createnewtag")]
        [Authorize]
        public async Task<ActionResult<string>> NewTag(TagDto tagDto)
        {
            Tag tag = new Tag();

            tag.Name = tagDto.Name;
            tag.Priority = tagDto.Priority;

            try
            {
                _dbcontext.Tags.Add(tag);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpPost]
        [Route("edittag")]
        [Authorize]
        public async Task<ActionResult<string>> EditTag(Tag tag)
        {
            try
            {
                _dbcontext.Tags.Update(tag);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpGet]
        [Authorize]
        [Route("getnumberoftags")]
        public async Task<ActionResult<int>> GetNumberOfTags()
        {
            int numberOfRecipes = _dbcontext.Tags.Count();
            return Ok(numberOfRecipes);
        }

        // Users

        [HttpPost]
        [Route("getusers")]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetUsers(PaginatorDto paginatorDto)
        {
            List<User> pageList = new List<User>();
            var query = _dbcontext.Users.AsQueryable();

            try
            {
                query = query.Where(t => t.Login.StartsWith(paginatorDto.name));

                switch (paginatorDto.sort)
                {
                    case 1:
                        query = query.OrderBy(t => t.Name);
                        break;
                    case 2:
                        query = query.OrderByDescending(t => t.Name);
                        break;
                    case 3:
                        query = query.OrderBy(t => t.Login);
                        break;
                    case 4:
                        query = query.OrderByDescending(t => t.Login);
                        break;
                    case 5:
                        query = query.OrderBy(t => t.Role);
                        break;
                    case 6:
                        query = query.OrderByDescending(t => t.Role);
                        break;
                    case 7:
                        query = query.OrderBy(t => t.CreateDate);
                        break;
                    case 8:
                        query = query.OrderByDescending(t => t.CreateDate);
                        break;
                    default:
                        query = query.OrderBy(t => t.IdUser);
                        break;
                }
                pageList = query.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                           .Take(paginatorDto.PageSize).ToList<User>();
            }
            catch
            {
                return Ok(pageList);
            }

            return Ok(pageList);
        }

        [HttpPost]
        [Route("createnewuser")]
        [Authorize]
        public async Task<ActionResult<string>> NewUser(User user)
        {
            try
            {
                _dbcontext.Users.Add(user);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpPost]
        [Route("edituser")]
        [Authorize]
        public async Task<ActionResult<string>> EditUser(User user)
        {
            try
            {
                _dbcontext.Users.Update(user);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpGet]
        [Authorize]
        [Route("getnumberofusers")]
        public async Task<ActionResult<int>> GetNumberOfUsers()
        {
            int numberOfUsers = _dbcontext.Users.Count();
            return Ok(numberOfUsers);
        }

        //Recipes
        [HttpPost]
        [Route("getrecipes")]
        [Authorize]
        public async Task<ActionResult<List<Recipe>>> GetRecipes(PaginatorDto paginatorDto)
        {
            List<Recipe> pageList = new List<Recipe>();
            var query = _dbcontext.Recipes.AsQueryable();

            try
            {
                query = _dbcontext.Recipes
                            .Include(t => t.TagsIdTags)
                            .Include(t => t.ListsOfIngredients)
                            .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                            .Where(r => r.Name.StartsWith(paginatorDto.name));

                if (!paginatorDto.filtr.IsNullOrEmpty())
                {
                    query = query.Where(r => r.TagsIdTags.Any(t => paginatorDto.filtr.Contains(t.IdTag)));
                }


                switch (paginatorDto.sort)
                {
                    case 1:
                        query = query.OrderBy(r => r.Name);                   
                        break;
                    case 2:
                        query = query.OrderByDescending(r => r.Name);
                        break;
                    case 3:
                        query = query.OrderBy(r => r.PrepTime);
                        break;
                    case 4:
                        query = query.OrderByDescending(r => r.PrepTime);
                        break;
                    case 5:
                        query = query.OrderBy(r => r.IdRecipe);
                        break;
                    case 6:
                        query = query.OrderByDescending(r => r.IdRecipe);
                        break;
                    case 7:
                        query = query.OrderBy(r => r.CreateDate);
                        break;
                    case 8:
                        query = query.OrderByDescending(r => r.CreateDate);
                        break;
                    default:
                        query = query.OrderBy(r => r.IdRecipe);
                        break;
                }
                pageList = query.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                            .Take(paginatorDto.PageSize).ToList();
            } catch
            {
                return Ok(pageList);
            }
            return Ok(pageList);
        }

        [HttpPost]
        [Route("createnewrecipe")]
        [Authorize]
        public async Task<ActionResult<string>> NewRecipe(RecipeDto recipeDto)
        {
            Recipe addRecipe = new Recipe();

            String userName = HttpContext.User.Identity.Name;

            addRecipe.Name = recipeDto.Name;
            addRecipe.Description = recipeDto.Description;
            addRecipe.Instruction = recipeDto.Instruction;
            addRecipe.PrepTime = recipeDto.PrepTime;
            addRecipe.CreateDate = DateTime.Now;
            addRecipe.EditDate = DateTime.Now;
            addRecipe.UsersIdUserNavigation = _dbcontext.Users.Where(e => e.Login == userName).FirstOrDefault();

            recipeDto.tags.ForEach(tag =>
            {
                _dbcontext.Attach(tag);
                addRecipe.TagsIdTags.Add(tag);
            });

            recipeDto.ingredients.ForEach(ingredient =>
            {
                _dbcontext.Attach(ingredient.Grocery);
                addRecipe.ListsOfIngredients.Add(new ListsOfIngredient()
                {
                    Quanity = ingredient.Quantity,
                    GroceriesIdFoodItemNavigation = ingredient.Grocery
                });
            });

            try
            {
                _dbcontext.Recipes.Add(addRecipe);
                _dbcontext.SaveChanges();
                _dbcontext.ChangeTracker.Clear();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpPost]
        [Route("editrecipe")]
        [Authorize]
        public async Task<ActionResult<string>> EditRecipe(RecipeDto recipeDto)
        {

            Recipe editedRecord = _dbcontext.Recipes.Include(t => t.TagsIdTags).Include(t => t.ListsOfIngredients).ThenInclude(i => i.GroceriesIdFoodItemNavigation).Where(r => r.IdRecipe == recipeDto.RecipeId).FirstOrDefault();

            editedRecord.IdRecipe = recipeDto.RecipeId;
            editedRecord.Name = recipeDto.Name;
            editedRecord.Description = recipeDto.Description;
            editedRecord.Instruction = recipeDto.Instruction;
            editedRecord.PrepTime = recipeDto.PrepTime;
            editedRecord.CreateDate = recipeDto.CreateDate;
            editedRecord.EditDate = DateTime.Now;

            editedRecord.TagsIdTags.Clear();

            _dbcontext.SaveChanges();

            _dbcontext.ChangeTracker.Clear();

            editedRecord.TagsIdTags = recipeDto.tags;

            List<ListsOfIngredient> deleteIngredients = editedRecord.ListsOfIngredients.ToList();

            recipeDto.ingredients.ForEach(ingredient =>
            {
                ListsOfIngredient listsOfIngredient = new ListsOfIngredient()
                {
                    Quanity = ingredient.Quantity,
                    GroceriesIdFoodItemNavigation = ingredient.Grocery
                };

                ListsOfIngredient editedIngredients = editedRecord.ListsOfIngredients.Where(l => l.GroceriesIdFoodItemNavigation.IdFoodItem == ingredient.Grocery.IdFoodItem).FirstOrDefault();

                deleteIngredients.Remove(editedIngredients);

            if (editedIngredients == null)
                {
                    editedRecord.ListsOfIngredients.Add(new ListsOfIngredient()
                    {
                        Quanity = ingredient.Quantity,
                        GroceriesIdFoodItemNavigation = ingredient.Grocery
                    });
                } else if (editedIngredients.Quanity != listsOfIngredient.Quanity)
                {
                    editedIngredients.Quanity = listsOfIngredient.Quanity;
                }  
            });

            deleteIngredients.ForEach(d =>
            {
                editedRecord.ListsOfIngredients.Remove(d);
                _dbcontext.ListsOfIngredients.Remove(d);
            });

            try
            {
                _dbcontext.Set<Recipe>().Update(editedRecord);
                _dbcontext.SaveChanges();
            }
            catch (Exception x)
            {
                return Ok(x.Message);
            }

            return Ok("");
        }

        [HttpGet]
        [Authorize]
        [Route("getnumberofrecipes")]
        public async Task<ActionResult<int>> GetNumberOfRecipes()
        {
            int numberOfRecipes = _dbcontext.Recipes.Count();
            return Ok(numberOfRecipes);
        }

        //Groceries

        [HttpPost]
        [Route("getgroceries")]
        [Authorize]
        public async Task<ActionResult<List<Grocery>>> GetGroceries(PaginatorDto paginatorDto)
        {
            List<Grocery> pageList = new List<Grocery>();
            var query = _dbcontext.Groceries.AsQueryable();

            try
            {
                query = query.Where(g => g.Name.StartsWith(paginatorDto.name));

                switch (paginatorDto.sort)
                {
                    case 1:
                        query = query.OrderBy(g => g.Name);
                        break;
                    case 2:
                        query = query.OrderByDescending(g => g.Name);
                        break;
                    case 3:
                        query = query.OrderBy(g => g.Unit);
                        break;
                    case 4:
                        query = query.OrderByDescending(g => g.Unit);
                        break;
                    default:
                        query = query.OrderBy(g => g.IdFoodItem);
                        break;
                }
                pageList = query.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                            .Take(paginatorDto.PageSize).ToList<Grocery>();
            }
            catch
            {
                return Ok(pageList);
            }

            return Ok(pageList);
        }

        [HttpPost]
        [Route("createnewgrocery")]
        [Authorize]
        public async Task<ActionResult<string>> NewGrocery(Grocery grocery)
        {
            try
            {
                _dbcontext.Groceries.Add(grocery);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpPost]
        [Route("editgrocery")]
        [Authorize]
        public async Task<ActionResult<string>> EditGrocery(Grocery grocery)
        {
            try
            {
                _dbcontext.Groceries.Update(grocery);
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpGet]
        [Authorize]
        [Route("getnumberofgroceries")]
        public async Task<ActionResult<int>> GetNumberOfGroceries()
        {
            int numberOfGroceries = _dbcontext.Groceries.Count();
            return Ok(numberOfGroceries);
        }
    }
}

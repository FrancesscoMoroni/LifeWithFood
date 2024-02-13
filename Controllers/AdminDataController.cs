using Azure;
using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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

        [HttpPost]
        [Authorize(Roles = "1")]
        [Route("deletetag")]
        public async Task<ActionResult<String>> DeleteTag(IdDto tagId)
        {
            try
            {
                _dbcontext.Tags
                    .Remove(_dbcontext.Tags
                        .Where(t => t.IdTag == tagId.Id)
                        .FirstOrDefault()
                    );
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd podczas usuwania");
            }

            return Ok("");
        }

        // Users

        [HttpPost]
        [Route("getusers")]
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
        public async Task<ActionResult<List<RecipeDto>>> GetRecipes(PaginatorDto paginatorDto)
        {
            List<RecipeDto> pageList = new List<RecipeDto>();
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

                var queryRecipeDto = query.Select(s => new RecipeDto
                {
                    IdRecipe = s.IdRecipe,
                    Name = s.Name,
                    Description = s.Description,
                    Instruction = s.Instruction,
                    PrepTime = s.PrepTime,
                    CreateDate = DateOnly.FromDateTime(s.CreateDate),
                    EditDate = DateOnly.FromDateTime((DateTime)s.EditDate),
                    Tags = s.TagsIdTags.ToList(),
                    Creator = s.UsersIdUserNavigation.Login,
                    ListsOfIngredients = s.ListsOfIngredients,
                    Score = s.Ratings.Count != 0 ? s.Ratings.Sum(r => r.Score) / s.Ratings.Count : 0,
                });

                pageList = queryRecipeDto.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                            .Take(paginatorDto.PageSize).ToList();
            } catch
            {
                return Ok(pageList);
            }
            return Ok(pageList);
        }

        [HttpPost]
        [Route("createnewrecipe")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<string>> NewRecipe(RecipeEditDto recipeEditDto)
        {
            Recipe addRecipe = new Recipe();

            String userName = HttpContext.User.Identity.Name;

            addRecipe.Name = recipeEditDto.Name;
            addRecipe.Description = recipeEditDto.Description;
            addRecipe.Instruction = recipeEditDto.Instruction;
            addRecipe.PrepTime = recipeEditDto.PrepTime;
            addRecipe.CreateDate = DateTime.Now;
            addRecipe.EditDate = DateTime.Now;
            addRecipe.UsersIdUserNavigation = _dbcontext.Users.Where(e => e.Login == userName).FirstOrDefault();

            recipeEditDto.tags.ForEach(tag =>
            {
                _dbcontext.Attach(tag);
                addRecipe.TagsIdTags.Add(tag);
            });

            recipeEditDto.ingredients.ForEach(ingredient =>
            {
                _dbcontext.Attach(ingredient.Grocery);
                addRecipe.ListsOfIngredients.Add(new ListsOfIngredient()
                {
                    Quantity = ingredient.Quantity,
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
        [Authorize(Roles = "1")]
        public async Task<ActionResult<string>> EditRecipe(RecipeEditDto recipeEditDto)
        {

            Recipe editedRecord = _dbcontext.Recipes
                .Include(t => t.TagsIdTags)
                .Include(t => t.ListsOfIngredients)
                .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                .Where(r => r.IdRecipe == recipeEditDto.IdRecipe)
                .FirstOrDefault();

            editedRecord.IdRecipe = recipeEditDto.IdRecipe;
            editedRecord.Name = recipeEditDto.Name;
            editedRecord.Description = recipeEditDto.Description;
            editedRecord.Instruction = recipeEditDto.Instruction;
            editedRecord.PrepTime = recipeEditDto.PrepTime;
            editedRecord.EditDate = DateTime.Now;


            editedRecord.TagsIdTags.Clear();

            editedRecord.ListsOfIngredients.Clear();

            _dbcontext.SaveChanges();

            _dbcontext.ChangeTracker.Clear();

            editedRecord.TagsIdTags = recipeEditDto.tags;

            recipeEditDto.ingredients.ForEach(i =>
            {
                editedRecord.ListsOfIngredients.Add(new ListsOfIngredient
                {
                    Quantity = i.Quantity,
                    GroceriesIdFoodItemNavigation = i.Grocery
                });
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

        [HttpPost]
        [Authorize(Roles = "1")]
        [Route("deleterecipe")]
        public async Task<ActionResult<String>> DeleteRecipe(IdDto idRecipe)
        {
            try
            {
                _dbcontext
                    .Remove(_dbcontext.Recipes
                        .Where(r => r.IdRecipe == idRecipe.Id)
                        .FirstOrDefault()
                    );
                _dbcontext.SaveChanges();
            } catch
            {
                return Ok("Błąd podczas usuwania");
            }

            return Ok("");
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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

        [HttpPost]
        [Authorize(Roles = "1")]
        [Route("deletegrocery")]
        public async Task<ActionResult<String>> DeleteGrocery(IdDto groceryId)
        {
            try
            {
                _dbcontext.Groceries
                    .Remove(_dbcontext.Groceries
                        .Where(g => g.IdFoodItem == groceryId.Id)
                        .FirstOrDefault()
                    );
                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd podczas usuwania");
            }

            return Ok("");
        }
    }
}

using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly LifeWithFoodDbContext _dbcontext;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters tokenValidationParameters;

        public DataController(LifeWithFoodDbContext dbcontext, IConfiguration configuration)
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

        // Ingredients

        [HttpGet]
        [Route("getingredientsselect")]
        public async Task<ActionResult<List<SelectIngredientDto>>> GetIngredientsSelect()
        {
            List<SelectIngredientDto> ingredientsSelect = new List<SelectIngredientDto>(); 

            List<Grocery> list = new List<Grocery>();

            try
            {
                list = _dbcontext.Groceries.OrderBy(item => item.Name).ToList<Grocery>();
            }
            catch
            {
                return Ok(list);
            }

            list.ForEach( item =>
            {
                ingredientsSelect.Add(new SelectIngredientDto() { Name = item.Name, Grocery = item});
            });

            return Ok(ingredientsSelect);
        }

        //Tags


        [HttpGet]
        [Route("gettagselect")]
        public async Task<ActionResult<List<SelectTagDto>>> GetTagSelect()
        {
            List<SelectTagDto> tagSelect = new List<SelectTagDto>();

            List<Tag> list = new List<Tag>();

            try
            {
                list = _dbcontext.Tags.OrderBy(item => item.Name).ToList<Tag>();
            }
            catch
            {
                return Ok(list);
            }

            list.ForEach(item =>
            {
                tagSelect.Add(new SelectTagDto() { Name = item.Name, Tag = item });
            });

            return Ok(tagSelect);
        }

        [HttpPost]
        [Route("getrecipes")]
        [Authorize]
        public async Task<ActionResult<List<RecipeDto>>> GetRecipes(PaginatorDto paginatorDto)
        {
            List<RecipeDto> pageList = new List<RecipeDto>();
            var query = _dbcontext.Recipes.AsQueryable();
            String userName =  HttpContext.User.Identity.Name;

            try
            {
                query = _dbcontext.Recipes
                            .Include(t => t.TagsIdTags)
                            .Include(t => t.Ratings)
                            .Include(t => t.ListsOfIngredients)
                            .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                            .Where(r => r.UsersIdUserNavigation.Login == userName)
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
            }
            catch
            {
                return Ok(pageList);
            }
            return Ok(pageList);
        }

        [HttpPost]
        [Route("createnewrecipe")]
        [Authorize]
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
        [Authorize]
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

            recipeEditDto.ingredients.ForEach( i =>
            {
                editedRecord.ListsOfIngredients.Add( new ListsOfIngredient
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

        //Storeroom

        [HttpPost]
        [Route("createnewstoreroomitem")]
        [Authorize]
        public async Task<ActionResult<string>> NewStoreroomItem(OwnedGroceryDto ownedGroceryDto)
        {
            OwnedGrocery addOwnedGrocery = new OwnedGrocery();

            String userName = HttpContext.User.Identity.Name;
         
            try
            {
                addOwnedGrocery.Location = ownedGroceryDto.Location;
                addOwnedGrocery.Quantity = ownedGroceryDto.Quantity;
                addOwnedGrocery.ExpirationDate = ownedGroceryDto.ExpirationDate.ToDateTime(TimeOnly.MinValue);
                addOwnedGrocery.GroceriesIdFoodItemNavigation = _dbcontext.Groceries
                    .Where(g => g.IdFoodItem == ownedGroceryDto.Grocery.IdFoodItem).FirstOrDefault();

                _dbcontext.Users.Where(u => u.Login == userName)
                    .FirstOrDefault()
                    .OwnedGroceries.Add(addOwnedGrocery);

                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpPost]
        [Route("editstoreroomitem")]
        [Authorize]
        public async Task<ActionResult<string>> EditStoreroomItem(OwnedGroceryDto ownedGroceryDto)
        {

            OwnedGrocery editOwnedGrocery = new OwnedGrocery();

            try
            {
                editOwnedGrocery = _dbcontext.OwnedGroceries.Where(o => o.IdOwnedFoodItem == ownedGroceryDto.IdOwnedFoodItem).FirstOrDefault();

                editOwnedGrocery.Quantity = ownedGroceryDto.Quantity;
                editOwnedGrocery.Location = ownedGroceryDto.Location;
                editOwnedGrocery.ExpirationDate = ownedGroceryDto.ExpirationDate.ToDateTime(TimeOnly.MinValue);

                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpPost]
        [Route("deletestoreroomitem")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteStoreroomItem(OwnedGroceryDto ownedGroceryDto)
        {

            OwnedGrocery deleteOwnedGrocery = _dbcontext.OwnedGroceries.Where(o => o.IdOwnedFoodItem == ownedGroceryDto.IdOwnedFoodItem).FirstOrDefault();

            try
            {
                _dbcontext.OwnedGroceries
                    .Remove(deleteOwnedGrocery);

                _dbcontext.SaveChanges();
            }
            catch
            {
                return Ok("Błąd dodawania");
            }

            return Ok("");
        }

        [HttpGet]
        [Route("getstoreroomitems")]
        [Authorize]
        public async Task<ActionResult<StoreroomDto>> GetStoreroomItems()
        {
            List<OwnedGroceryDto> storeRoomGroceries = new List<OwnedGroceryDto>();
            List<string> storeRoomNames = new List<string>();
            StoreroomDto storeroomData = new StoreroomDto();

            String userName = HttpContext.User.Identity.Name;

            try
            {
                storeRoomNames = _dbcontext.Users
                    .Include(u => u.OwnedGroceries)
                    .ThenInclude(o => o.GroceriesIdFoodItemNavigation)
                    .Where(u => u.Login == userName)
                    .FirstOrDefault()
                    .OwnedGroceries
                    .Select(o => o.Location)
                    .Distinct()
                    .ToList();

                storeRoomGroceries = _dbcontext.Users
                    .Include(u => u.OwnedGroceries)
                    .ThenInclude(o => o.GroceriesIdFoodItemNavigation)
                    .Where(u => u.Login == userName)                  
                    .FirstOrDefault()
                    .OwnedGroceries
                    .Select(s => new OwnedGroceryDto {
                        IdOwnedFoodItem = s.IdOwnedFoodItem,
                        Location = s.Location,
                        Quantity = s.Quantity,
                        ExpirationDate = DateOnly.FromDateTime((DateTime)s.ExpirationDate),
                        Grocery = new GroceryDto { 
                            IdFoodItem = s.GroceriesIdFoodItemNavigation.IdFoodItem,
                            Name = s.GroceriesIdFoodItemNavigation.Name,
                            Unit = s.GroceriesIdFoodItemNavigation.Unit
                        }
                    })
                    .ToList();

                storeroomData.Locations = storeRoomNames;
                storeroomData.OwnedGroceries = storeRoomGroceries;
            }
            catch
            {
                return Ok(storeroomData);
            }

            return Ok(storeroomData);
        }

        [HttpPost]
        [Route("getfavoriterecipes")]
        [Authorize]
        public async Task<ActionResult<List<RecipeDto>>> GetFavoriteRecipes(PaginatorDto paginatorDto)
        {
           
            List<RecipeDto> pageList = new List<RecipeDto>();
            var query = _dbcontext.Recipes.AsQueryable();
            String userName = HttpContext.User.Identity.Name;

            try
            {
                User currentUser = _dbcontext.Users.Where(u => u.Login == userName).FirstOrDefault();

                query = _dbcontext.Recipes
                            .Include(t => t.TagsIdTags)
                            .Include(t => t.ListsOfIngredients)
                            .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                            .Where(r => r.UsersIdUsers.Contains(currentUser))
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
            }
            catch
            {
                return Ok(pageList);
            }
            return Ok(pageList);
        }

        [HttpGet]
        [Authorize]
        [Route("getnumberoffavoriterecipes")]
        public async Task<ActionResult<int>> GetNumberOfFavoriteRecipes()
        {

            int numberOfRecipes = 0;
            String userName = HttpContext.User.Identity.Name;

            try
            {
                User currentUser = _dbcontext.Users.Where(u => u.Login == userName).FirstOrDefault();

                numberOfRecipes = _dbcontext.Recipes
                    .Include(r => r.UsersIdUsers)
                    .Where(r => r.UsersIdUsers.Contains(currentUser))
                    .Count();
            } catch
            {
                return Ok(numberOfRecipes);
            }
            
            return Ok(numberOfRecipes);
        }

        [HttpPost]
        [Authorize]
        [Route("whatcanyoucook")]
        public async Task<ActionResult<List<RecipeYouCanCook>>> WhatCanYouCook(WhatCanYouCookDto whatCanYouCookDto)
        {
            String userName = HttpContext.User.Identity.Name;

            List<RecipeYouCanCook> allRecipes = new List<RecipeYouCanCook>();
            List<RecipeYouCanCook> recipesYouCanCook = new List<RecipeYouCanCook>();

            List<RecipeIngredientDto> userIngredients = new List<RecipeIngredientDto>();

            var query = _dbcontext.Recipes.AsQueryable();

            try
            {
                userIngredients = _dbcontext.Users
                    .Where(u => u.Login == userName)
                    .Include(u => u.OwnedGroceries)
                    .ThenInclude(o => o.GroceriesIdFoodItemNavigation)
                    .FirstOrDefault()
                    .OwnedGroceries
                    .GroupBy(o => o.GroceriesIdFoodItemNavigation.Name)
                    .Select(o => new RecipeIngredientDto
                    {
                        IdFoodItem = o.Where(t => t.GroceriesIdFoodItemNavigation.Name == o.Key).First().GroceriesIdFoodItemNavigation.IdFoodItem,
                        Name = o.Key,
                        Quantity = o.Sum(o => o.Quantity),
                        Unit = o.Where(t => t.GroceriesIdFoodItemNavigation.Name == o.Key).First().GroceriesIdFoodItemNavigation.Unit
                    })
                    .ToList();

                query = query.Include(t => t.TagsIdTags)
                    .Include(r => r.ListsOfIngredients)
                    .ThenInclude(i => i.GroceriesIdFoodItemNavigation);

                if (!whatCanYouCookDto.filtr.IsNullOrEmpty())
                {
                    query = query.Where(r => r.TagsIdTags.Any(t => whatCanYouCookDto.filtr.Contains(t.IdTag)));
                }

                allRecipes = query
                    .Select(r => new RecipeYouCanCook
                    {
                        RecipeCard = new RecipeCardDto
                        {
                            Id = r.IdRecipe,
                            Name = r.Name,
                            Description = r.Description,
                            PrepTime = r.PrepTime,
                            Tags = r.TagsIdTags.Select(t => new TagDto { Name = t.Name, Priority = t.Priority}).ToList(),
                            Score = r.Ratings.Count != 0 ? r.Ratings.Sum(r => r.Score) / r.Ratings.Count : 0
                        },
                        AllIngredients = r.ListsOfIngredients.Select(l => new RecipeIngredientDto {
                                IdFoodItem = l.GroceriesIdFoodItem,
                                Name = l.GroceriesIdFoodItemNavigation.Name,
                                Quantity = l.Quantity,
                                Unit = l.GroceriesIdFoodItemNavigation.Unit
                            }).ToList()
                    })
                    .ToList();

                

                allRecipes.ForEach(r =>
                {
                    List<RecipeIngredientDto> intersectIngredients = userIngredients.Where(u => r.AllIngredients
                         .Contains(r.AllIngredients
                             .Where(a => a.IdFoodItem == u.IdFoodItem).FirstOrDefault()
                          )
                        )
                        .Select(u => new RecipeIngredientDto {
                            IdFoodItem = u.IdFoodItem,
                            Name = u.Name,
                            Quantity = u.Quantity > r.AllIngredients.Where(a => a.IdFoodItem == u.IdFoodItem)
                                .FirstOrDefault().Quantity ? r.AllIngredients
                                .Where(a => a.IdFoodItem == u.IdFoodItem)
                                .FirstOrDefault().Quantity : u.Quantity,
                            Unit = u.Unit
                        })
                        .ToList();

                    if (!intersectIngredients.IsNullOrEmpty())
                    {
                        double recipeIngredientsSum = r.AllIngredients.Sum(a => a.Quantity);
                        double userIngredientsSum = intersectIngredients.Sum(i => i.Quantity);

                        

                        double ingredientsStatus = (userIngredientsSum / recipeIngredientsSum) * 100.0;

                        if (ingredientsStatus > 50.0 )
                        {
                            r.Compatibility = ((int)ingredientsStatus);

                            recipesYouCanCook.Add(r);
                        }
                    }
                });
            }
            catch
            {
                return Ok(recipesYouCanCook);
            }

            return Ok(recipesYouCanCook.OrderByDescending(r => r.Compatibility).Take(whatCanYouCookDto.Amount));
        }
    }
}

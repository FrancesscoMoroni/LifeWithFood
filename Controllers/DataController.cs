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
                list = _dbcontext.Groceries.OrderBy(item => item.IdFoodItem).ToList<Grocery>();
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
                list = _dbcontext.Tags.OrderBy(item => item.IdTag).ToList<Tag>();
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
        public async Task<ActionResult<List<Recipe>>> GetRecipes(PaginatorDto paginatorDto)
        {
            List<Recipe> pageList = new List<Recipe>();
            var query = _dbcontext.Recipes.AsQueryable();
            String userName =  HttpContext.User.Identity.Name;

            try
            {
                query = _dbcontext.Recipes
                            .Include(t => t.TagsIdTags)
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
                pageList = query.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
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

            Recipe editedRecord = _dbcontext.Recipes
                .Include(t => t.TagsIdTags)
                .Include(t => t.ListsOfIngredients)
                .ThenInclude(i => i.GroceriesIdFoodItemNavigation)
                .Where(r => r.IdRecipe == recipeDto.IdRecipe)
                .FirstOrDefault();

            editedRecord.IdRecipe = recipeDto.IdRecipe;
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
                }
                else if (editedIngredients.Quanity != listsOfIngredient.Quanity)
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

        //Storeroom

        [HttpPost]
        [Route("createnewstoreroomitem")]
        [Authorize]
        public async Task<ActionResult<string>> NewStoreroomItem(StoreroomItemDto storeroomDto)
        {
            OwnedGrocery addOwnedGrocery = new OwnedGrocery();

            String userName = HttpContext.User.Identity.Name;
         
            try
            {
                addOwnedGrocery.Location = storeroomDto.Location;
                addOwnedGrocery.Quanity = storeroomDto.Quantity;
                addOwnedGrocery.ExpirationDate = storeroomDto.ExpirationDate;
                addOwnedGrocery.GroceriesIdFoodItemNavigation = storeroomDto.Grocery;

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
        public async Task<ActionResult<string>> EditStoreroomItem(StoreroomItemDto storeroomDto)
        {

            OwnedGrocery editOwnedGrocery = new OwnedGrocery();

            try
            {
                editOwnedGrocery = _dbcontext.OwnedGroceries.Where(o => o.IdOwnedFoodItem == storeroomDto.Id).FirstOrDefault();

                editOwnedGrocery.Quanity = storeroomDto.Quantity;
                editOwnedGrocery.Location = storeroomDto.Location;
                editOwnedGrocery.ExpirationDate = storeroomDto.ExpirationDate;

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
        public async Task<ActionResult<string>> DeleteStoreroomItem(StoreroomItemDto storeroomDto)
        {

            OwnedGrocery deleteOwnedGrocery = _dbcontext.OwnedGroceries.Where(o => o.IdOwnedFoodItem == storeroomDto.Id).FirstOrDefault();

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
            List<OwnedGrocery> storeRoomGroceries = new List<OwnedGrocery>();
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
        public async Task<ActionResult<List<Recipe>>> GetFavoriteRecipes(PaginatorDto paginatorDto)
        {
           
            List<Recipe> pageList = new List<Recipe>();
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
                pageList = query.Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
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
        public async Task<ActionResult<List<RecipeIngredientDto>>> WhatCanYouCook(WhatCanYouCookDto whatCanYouCookDto)
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
                        Quantity = o.Sum(o => o.Quanity),
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
                            Tags = r.TagsIdTags.Select(t => new TagDto { Name = t.Name, Priority = t.Priority}).ToList()
                        },
                        AllIngredients = r.ListsOfIngredients.Select(l => new RecipeIngredientDto {
                                IdFoodItem = l.GroceriesIdFoodItem,
                                Name = l.GroceriesIdFoodItemNavigation.Name,
                                Quantity = l.Quanity,
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
                                                                   Quantity = u.Quantity > r.AllIngredients.Where(a => a.IdFoodItem == u.IdFoodItem).FirstOrDefault().Quantity ? r.AllIngredients.Where(a => a.IdFoodItem == u.IdFoodItem).FirstOrDefault().Quantity : u.Quantity,
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

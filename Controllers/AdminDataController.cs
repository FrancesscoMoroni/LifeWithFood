using LifeWithFood.Data;
using LifeWithFood.DTO;
using LifeWithFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LifeWithFood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminDataController : ControllerBase
    {

        private readonly LifeWithFoodDbContext _dbcontext;

        public AdminDataController(LifeWithFoodDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // Tags

        [HttpPost]
        [Route("gettags")]
        [Authorize]
        public async Task<ActionResult<List<Tag>>> GetTags(PaginatorDto paginatorDto)
        {
            List<Tag> pageList = new List<Tag>();

            try
            {
                pageList = _dbcontext.Tags.OrderBy(item => item.IdTag).Skip(paginatorDto.PageIndex * paginatorDto.PageSize).Take(paginatorDto.PageSize).ToList<Tag>();
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

            try
            {
                pageList = _dbcontext.Users.OrderBy(item => item.IdUser).Skip(paginatorDto.PageIndex * paginatorDto.PageSize).Take(paginatorDto.PageSize).ToList<User>();
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

            try
            {
                pageList = _dbcontext.Recipes.OrderBy(item => item.IdRecipe).Skip(paginatorDto.PageIndex * paginatorDto.PageSize).Take(paginatorDto.PageSize).ToList<Recipe>();
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
        public async Task<ActionResult<string>> NewRecipe(Recipe recipe)
        {
            try
            {
                _dbcontext.Recipes.Add(recipe);
                _dbcontext.SaveChanges();
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
        public async Task<ActionResult<string>> EditRecipe(Recipe recipe)
        {
            try
            {
                _dbcontext.Recipes.Update(recipe);
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

            try
            {
                pageList = _dbcontext.Groceries.OrderBy(item => item.IdFoodItem).Skip(paginatorDto.PageIndex * paginatorDto.PageSize).Take(paginatorDto.PageSize).ToList<Grocery>();
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

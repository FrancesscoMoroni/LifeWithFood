﻿using LifeWithFood.Data;
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
                pageList = query.Select(s => new RecipeCardDto {
                        Name = s.Name,
                        Description = s.Description,
                        Id = s.IdRecipe,
                        PrepTime = s.PrepTime,
                        Tags = s.TagsIdTags.Select(t => new TagDto
                        {
                            Name = t.Name,
                            Priority = t.Priority
                        }).OrderBy(t => t.Priority).ToList()
                    })
                    .Skip(paginatorDto.PageIndex * paginatorDto.PageSize)
                    .Take(paginatorDto.PageSize)
                    .ToList<RecipeCardDto>();

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
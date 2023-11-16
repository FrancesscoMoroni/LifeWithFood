using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipeCardDto
    {
        public Recipe Recipe { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
    }
}

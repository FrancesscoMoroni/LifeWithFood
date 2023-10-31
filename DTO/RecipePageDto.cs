using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipePageDto
    {
        public Recipe Recipe { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
    }
}

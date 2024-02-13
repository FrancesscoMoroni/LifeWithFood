using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipeEditDto
    {
        public int IdRecipe { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public int PrepTime { get; set; }

        public List<Tag> tags { get; set; }

        public List<IngredientDto> ingredients { get; set;}

    }
}

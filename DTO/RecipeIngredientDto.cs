using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipeIngredientDto
    {
        public int IdFoodItem { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}

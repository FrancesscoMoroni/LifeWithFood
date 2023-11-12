using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class SelectIngredientDto
    {
        public string Name { get; set; }
        public Grocery Grocery { get; set; } 
    }
}

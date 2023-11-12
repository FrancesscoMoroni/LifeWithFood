using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class IngredientDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Grocery Grocery { get; set; }
    }
}

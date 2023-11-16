using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipePageDto
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public int? PrepTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public string CreatorName { get; set; }
        public List<TagDto> tags { get; set; }
        public List<RecipeIngredientDto> ingredients { get; set;}
        public List<RatingDto> ratings { get; set; }
        public int? finalScore { get; set; }

    }
}

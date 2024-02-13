using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipeCardDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public int? PrepTime { get; set; }
        public List<TagDto> Tags { get; set; }
        public int? Score { get; set; }
        public int? SumScore { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

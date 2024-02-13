using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class RecipeDto
    {
        public int IdRecipe { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Instruction { get; set; }

        public int? PrepTime { get; set; }

        public DateOnly CreateDate { get; set; }

        public DateOnly? EditDate { get; set; }

        public string Creator { get; set; }
        public int? Score { get; set; }

        public virtual ICollection<ListsOfIngredient> ListsOfIngredients { get; set; } = new List<ListsOfIngredient>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    }
}

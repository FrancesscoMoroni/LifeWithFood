namespace LifeWithFood.DTO
{
    public class RecipeYouCanCook
    {
        public RecipeCardDto RecipeCard { get; set; }
        public List<RecipeIngredientDto> AllIngredients {get; set;}
        public double Compatibility { get; set;}
    }
}

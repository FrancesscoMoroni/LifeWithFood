namespace LifeWithFood.DTO
{
    public class RatingDto
    {
        public int IdRating { get; set; }
        public int IdRecipe { get; set; }
        public int? Score { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }

    }
}

using LifeWithFood.Models;

namespace LifeWithFood.DTO
{
    public class StoreroomItemDto
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Grocery Grocery { get; set; }
    }
}

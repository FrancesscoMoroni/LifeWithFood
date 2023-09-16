using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class Recipe
{
    public int IdRecipe { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Instruction { get; set; }

    public int? PrepTime { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? EditDate { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Grocery> GroceriesIdFoodItems { get; set; } = new List<Grocery>();

    public virtual ICollection<Tag> TagsIdTags { get; set; } = new List<Tag>();

    public virtual ICollection<User> UsersIdUsers { get; set; } = new List<User>();
}

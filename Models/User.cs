using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public int? Role { get; set; }

    public virtual ICollection<OwnedGrocery> OwnedGroceries { get; set; } = new List<OwnedGrocery>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Recipe> RecipesIdRecipes { get; set; } = new List<Recipe>();
}

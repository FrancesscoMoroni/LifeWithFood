using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class Grocery
{
    public int IdFoodItem { get; set; }

    public string Name { get; set; }

    public virtual ICollection<OwnedGrocery> OwnedGroceries { get; set; } = new List<OwnedGrocery>();

    public virtual ICollection<Recipe> RecipesIdRecipes { get; set; } = new List<Recipe>();
}

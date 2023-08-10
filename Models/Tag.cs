using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class Tag
{
    public int IdTag { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Recipe> RecipesIdRecipes { get; set; } = new List<Recipe>();
}

using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class Image
{
    public int IdImage { get; set; }

    public string Position { get; set; }

    public int RecipesIdRecipe { get; set; }

    public virtual Recipe RecipesIdRecipeNavigation { get; set; }
}

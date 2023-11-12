using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class ListsOfIngredient
{
    public int GroceriesIdFoodItem { get; set; }

    public int RecipesIdRecipe { get; set; }

    public int Quanity { get; set; }

    public virtual Grocery GroceriesIdFoodItemNavigation { get; set; }

    public virtual Recipe RecipesIdRecipeNavigation { get; set; }
}

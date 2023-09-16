using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class Rating
{
    public int IdRating { get; set; }

    public int? Score { get; set; }

    public string Comment { get; set; }

    public DateTime Date { get; set; }

    public int UsersIdUser { get; set; }

    public int RecipesIdRecipe { get; set; }

    public virtual Recipe RecipesIdRecipeNavigation { get; set; }

    public virtual User UsersIdUserNavigation { get; set; }
}

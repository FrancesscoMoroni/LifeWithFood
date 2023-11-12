using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class Grocery
{
    public int IdFoodItem { get; set; }

    public string Name { get; set; }

    public string Unit { get; set; }

    public virtual ICollection<ListsOfIngredient> ListsOfIngredients { get; set; } = new List<ListsOfIngredient>();

    public virtual ICollection<OwnedGrocery> OwnedGroceries { get; set; } = new List<OwnedGrocery>();
}

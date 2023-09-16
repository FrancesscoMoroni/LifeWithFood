using System;
using System.Collections.Generic;

namespace LifeWithFood.Models;

public partial class OwnedGrocery
{
    public int IdOwnedFoodItem { get; set; }

    public string Name { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int UsersIdUser { get; set; }

    public int GroceriesIdFoodItem { get; set; }

    public virtual Grocery GroceriesIdFoodItemNavigation { get; set; }

    public virtual User UsersIdUserNavigation { get; set; }
}

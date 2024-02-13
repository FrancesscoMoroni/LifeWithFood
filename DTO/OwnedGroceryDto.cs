using System;
using System.Collections.Generic;

namespace LifeWithFood.DTO;

public partial class OwnedGroceryDto
{
    public int IdOwnedFoodItem { get; set; }

    public string Location { get; set; }

    public int Quantity { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public virtual GroceryDto Grocery { get; set; }
}

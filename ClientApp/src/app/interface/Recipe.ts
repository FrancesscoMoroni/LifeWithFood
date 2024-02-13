export interface Recipe {
  idRecipe: number;
  name: string;
  description: string;
  instruction: string;
  prepTime: number;
  createDate: Date;
  editDate: Date;
  creator: string;
  tags: [{
    idTag: number,
    priority: number,
    name: string
  }]
  listsOfIngredients: [{
    name: string,
    quantity: number,
    groceriesIdFoodItemNavigation: any
  }]
}

import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RecipePageService } from '../services/recipe-page.service';
import { Ingredient } from '../interface/Ingredient';

export interface DialogData {
  idRecipe: number,
  name: string
}

@Component({
  selector: 'shopping-list-dialog',
  templateUrl: './shopping-list-dialog.component.html',
  styleUrls: ['./shopping-list-dialog.component.css']
})
export class ShoppingListDialogComponent {
  title = "Składniki, które trzeba dokupić";

  ingredients: Array<Ingredient> = [];

  constructor(private recipePageService: RecipePageService, public dialogRef: MatDialogRef<ShoppingListDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.getIngredientsToBuy();
  }

  async getIngredientsToBuy() {
    this.ingredients = await this.recipePageService.checkOwnedIngredients(this.data.idRecipe);
  }

  onCancleClick(): void {
    this.dialogRef.close(false);
  }

  onPrintJsonClick(): void {
    if (this.ingredients?.length != 0) {
      const blob = new Blob([JSON.stringify(this.ingredients)], { type: 'txt' });
      let a = document.createElement('a');
      a.download = "Lista JSON na " + this.data.name;
      a.href = window.URL.createObjectURL(blob);

      a.click();
    }

    this.dialogRef.close(true);
  }

  onPrintClick(): void {
    if (this.ingredients?.length != 0) {
      var text = "Lista składników:";

      this.ingredients.forEach(i => {
        text += "\n - " + i.name + " " + i.quantity + " " + i.unit
      });
      
      const blob = new Blob([text], { type: 'txt' });
      let a = document.createElement('a');
      a.download = "Lista na " + this.data.name;
      a.href = window.URL.createObjectURL(blob);

      a.click();
    }

    this.dialogRef.close(true);
  }
}

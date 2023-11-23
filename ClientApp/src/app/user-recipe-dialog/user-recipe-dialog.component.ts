import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AddIngredientDialogComponent } from '../add-ingredient-dialog/add-ingredient-dialog.component';
import { AddTagDialogComponent } from '../add-tag-dialog/add-tag-dialog.component';
import { Recipe } from '../interface/Recipe';
import { DataService } from '../services/data-service.service';

export interface DialogData {
  mode: boolean;
  recipe: Recipe;
}
export interface Ingredients {
  name: string;
  grocery: any;
  quantity: number;
}

export interface Tag {
  name: string;
}


@Component({
  selector: 'user-recipe-dialog',
  templateUrl: './user-recipe-dialog.component.html',
  styleUrls: ['./user-recipe-dialog.component.css']
})
export class UserRecipeDialogComponent {
  ingredintsRows = ['name', 'quantity', 'options'];

  tagsRows = ['name', 'options'];

  ingredientsData: Ingredients[] = [];

  tagsData: any[] = [];

  ingredientsSource = new MatTableDataSource<Ingredients>(this.ingredientsData);
  tagsSource = new MatTableDataSource<any>(this.tagsData);


  okButtonName: string = 'Utwórz Nowy';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Nowy Przepis';

  recipeForm = this.fb.group({
    name: [this.data.recipe.name, Validators.required],
    description: [this.data.recipe.description, Validators.required],
    instruction: [this.data.recipe.instruction, Validators.required],
    prepTime: [this.data.recipe.prepTime, [Validators.required, Validators.pattern("^[0-9]*$")]],
  });

  constructor(private fb: FormBuilder, public dialog: MatDialog, public dialogRef: MatDialogRef<UserRecipeDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData, private dataService: DataService) {
    if (data.mode) {
      this.okButtonName = 'Utwórz Nowy';
      this.title = 'Nowy Przepis';
    } else {
      console.log(data);
      data.recipe.tagsIdTags.forEach(t => {
        this.tagsData.push({
          idTag: t.idTag,
          priority: t.priority,
          name: t.name
        });
      });

      data.recipe.listsOfIngredients.forEach(i => {
        this.ingredientsData.push({
          name: i.groceriesIdFoodItemNavigation.name,
          quantity: i.quanity,
          grocery: i.groceriesIdFoodItemNavigation
        });
      });

      this.tagsSource = new MatTableDataSource<any>(this.tagsData);
      this.okButtonName = 'Edytuj';
      this.title = 'Edytuj Przepis';
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    if (this.recipeForm.status == 'VALID') {
      if (this.data.mode) {
        var newRecipe = {
          name: this.recipeForm.value.name,
          instruction: this.recipeForm.value.instruction,
          description: this.recipeForm.value.description,
          prepTime: this.recipeForm.value.prepTime,
          tags: this.tagsData,
          ingredients: this.ingredientsData
        }

        this.dataService.createNewRecipe(newRecipe);
      } else {

        var editRecipe = {
          idRecipe: this.data.recipe.idRecipe,
          name: this.recipeForm.value.name,
          instruction: this.recipeForm.value.instruction,
          description: this.recipeForm.value.description,
          prepTime: this.recipeForm.value.prepTime,
          createDate: this.data.recipe.createDate,
          tags: this.tagsData,
          ingredients: this.ingredientsData
        }

        console.log(editRecipe);
        this.dataService.editRecipe(editRecipe);
      }

      this.dialogRef.close();
    }
  }
  editIngredient(ingredient: Ingredients) {
    const dialogRef = this.dialog.open(AddIngredientDialogComponent, { data: { mode: false, ingredient: ingredient } });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let foundElement = this.ingredientsData.find(e => e.grocery.idFoodItem == result.grocery.idFoodItem);

        if (foundElement != undefined) {
          foundElement.quantity = result.quantity;
        }
      }
    })
  }

  deleteIngredient(ingredient: Ingredients) {
    let index = this.ingredientsData.indexOf(ingredient);
    this.ingredientsData.splice(index, 1);

    this.ingredientsSource = new MatTableDataSource<Ingredients>(this.ingredientsData);
  }

  addIgredient() {
    const dialogRef = this.dialog.open(AddIngredientDialogComponent, { data: { mode: true } });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        let exist = false;
        this.ingredientsData.forEach(x => {
          if (result.grocery.idFoodItem == x.grocery.idFoodItem) {
            exist = true;
          }
        });

        if (!exist) {
          this.ingredientsData.push({
            name: result.grocery.name,
            grocery: result.grocery,
            quantity: result.quantity
          });

          this.ingredientsSource = new MatTableDataSource<Ingredients>(this.ingredientsData);
        }
      }
    });
  }

  deleteTag(tag: Tag) {
    let index = this.tagsData.indexOf(tag);
    this.tagsData.splice(index, 1);

    this.tagsSource = new MatTableDataSource<Tag>(this.tagsData);
  }

  addTag() {
    const dialogRef = this.dialog.open(AddTagDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let exist = false;
        this.tagsData.forEach(x => {
          if (x.idTag == result.tag.idTag) {
            exist = true;
          }
        });
        if (!exist) {
          this.tagsData.push(result.tag);
          this.tagsSource = new MatTableDataSource<Tag>(this.tagsData);
        }
      }
    });
  }
}

import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from '../services/data-service.service';

export interface IngredientData {
  ingredient: {
    grocery: Object,
    name: string,
    quantity: number
  },
  mode: boolean
}

@Component({
  selector: 'add-ingredient-dialog',
  templateUrl: './add-ingredient-dialog.component.html',
  styleUrls: ['./add-ingredient-dialog.component.css']
})
export class AddIngredientDialogComponent {
   
  okButtonName: string = 'Utwórz Nowy';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Dodaj Składnik';

  ingredientsSelection: any;

  ingredientForm = this.fb.group({
    grocery: ['', Validators.required],
    quantity: [this.data.ingredient?.quantity, [Validators.required, Validators.pattern("^[0-9]*$")]],
  });

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<AddIngredientDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any, public dataService: DataService) {
    this.getIngredients();
    if (data.mode) {
      this.okButtonName = 'Dodaj Nowy';
      this.title = 'Dodaj Składnik';
    } else {
      this.okButtonName = 'Edytuj';
      this.title = 'Edytuj Składnik';
    }
  }

  async getIngredients() {
    var result = await this.dataService.getIngredientsSelect();

    this.ingredientsSelection = result;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    if (this.ingredientForm.status == 'VALID') {
      this.dialogRef.close(this.ingredientForm.value);
    } else if (this.ingredientForm.get('quantity')!.status == 'VALID' && !this.data.mode) {
      this.ingredientForm.value.grocery = this.data.ingredient.grocery;
      this.dialogRef.close(this.ingredientForm.value);
    }    
  }
}

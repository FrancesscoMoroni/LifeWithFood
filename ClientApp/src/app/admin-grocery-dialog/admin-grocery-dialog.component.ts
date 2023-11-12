import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Grocery } from '../interface/Grocery';
import { AdminDataService } from '../services/admin-data.service';

export interface DialogData {
  mode: boolean;
  grocery: Grocery;
}

@Component({
  selector: 'admin-grocery-dialog',
  templateUrl: './admin-grocery-dialog.component.html',
  styleUrls: ['./admin-grocery-dialog.component.css']
})
export class AdminGroceryDialogComponent {
  okButtonName: string = 'Utwórz Nowy';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Nowy Składnik';

  groceryForm = this.fb.group({
    name: [this.data.grocery.name, Validators.required],
    unit: [this.data.grocery.unit, Validators.required],
  });

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<AdminGroceryDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData, private adminDataService: AdminDataService) {
    if (data.mode) {
      this.okButtonName = 'Utwórz Nowy';
      this.title = 'Nowy Składnik';
    } else {
      this.okButtonName = 'Edytuj';
      this.title = 'Edytuj Składnik';
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    if (this.groceryForm.status == 'VALID') {
      if (this.data.mode) {
        this.data.grocery.name = this.groceryForm.value.name!;
        this.data.grocery.unit = this.groceryForm.value.unit!;

        this.adminDataService.createNewGrocery(this.data.grocery);
      } else {
        this.data.grocery.name = this.groceryForm.value.name!;
        this.data.grocery.unit = this.groceryForm.value.unit!;

        this.adminDataService.editGrocery(this.data.grocery);
      }

      this.dialogRef.close();
    }
  }
}

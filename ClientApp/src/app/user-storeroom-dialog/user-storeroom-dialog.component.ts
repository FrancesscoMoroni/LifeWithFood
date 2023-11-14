import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { DataService } from '../services/data-service.service';

export interface DialogData {
  mode: boolean;
  data: {
    location: string,
    expirationDate: string,
    quanity: number,
    idOwnedFoodItem: number
  }
}

@Component({
  selector: 'user-storeroom-dialog',
  templateUrl: './user-storeroom-dialog.component.html',
  styleUrls: ['./user-storeroom-dialog.component.css']
})
export class UserStoreroomDialogComponent {
  ingredientsSelection: any;

  okButtonName: string = 'Dodaj';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Dodaj produkt';

  storeroomForm = this.fb.group({
    location: ['', Validators.required],
    expirationDate: ['', Validators.required],
    grocery: ['', Validators.required],
    quantity: ['', [Validators.required, Validators.pattern("^[0-9]*$")]]
  });

  constructor(private fb: FormBuilder, public dialog: MatDialog, public dialogRef: MatDialogRef<UserStoreroomDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData, private dataService: DataService) {
    this.getIngredients();
    if (data.mode) {
      this.okButtonName = 'Dodaj';
      this.title = 'Dodaj produkt';
    } else {

      this.storeroomForm.setValue({
        location: data.data.location,
        quantity: data.data.quanity.toString(),
        grocery: null,
        expirationDate: data.data.expirationDate.slice(0, 10)
      });

      this.okButtonName = 'Edytuj';
      this.title = 'Edytuj produkt';
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
    if (this.storeroomForm.status == 'VALID') {

      this.dataService.createNewStoreromeItem(this.storeroomForm.value);
      this.dialogRef.close();

    } else if (this.storeroomForm.get('quantity')!.status == 'VALID'
      && this.storeroomForm.get('location')!.status == 'VALID'
      && this.storeroomForm.get('expirationDate')!.status == 'VALID'
      && !this.data.mode) {

      var editStoreItem = {
        id: this.data.data.idOwnedFoodItem,
        location: this.storeroomForm.value.location,
        quantity: this.storeroomForm.value.quantity,
        expirationDate: this.storeroomForm.value.expirationDate
      }

      this.dataService.editStoreromeItem(editStoreItem);
      this.dialogRef.close();
    }
  }

 
}

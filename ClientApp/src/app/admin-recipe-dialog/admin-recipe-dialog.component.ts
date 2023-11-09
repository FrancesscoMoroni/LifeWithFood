import { Component, Inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'admin-recipe-dialog',
  templateUrl: './admin-recipe-dialog.component.html',
  styleUrls: ['./admin-recipe-dialog.component.css']
})
export class AdminRecipeDialogComponent {

  constructor(public dialogRef: MatDialogRef<AdminRecipeDialogComponent>) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

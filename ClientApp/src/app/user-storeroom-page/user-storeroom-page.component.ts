import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DataService } from '../services/data-service.service';
import { UserStoreroomDialogComponent } from '../user-storeroom-dialog/user-storeroom-dialog.component';
import { YesNoDialogComponent } from '../yes-no-dialog/yes-no-dialog.component';

interface StoreroomData {
  locations: any[],
  ownedGroceries: any[]
}

@Component({
  selector: 'app-user-storeroom-page',
  templateUrl: './user-storeroom-page.component.html',
  styleUrls: ['./user-storeroom-page.component.css']
})
export class UserStoreroomPageComponent {
  public unit = 'miara';
  public storeroomData: StoreroomData;


  constructor(public dialog: MatDialog, private fb: FormBuilder, private dataService: DataService) {
    this.storeroomData = {
      locations: [],
      ownedGroceries: []
    }

    this.getStoreroomItems();
  }

  addGrocery() {
    this.openDialog(true, '');
  }

  refreshStoreroom() {
    this.getStoreroomItems();
  }

  editStoreroomItem(ownedGrocery: any) {
    this.openDialog(false, ownedGrocery);
  }

  deleteStoreroomItem(ownedGroceryId: any) {
    const dialogRef = this.dialog.open(YesNoDialogComponent, { data: { title: 'usunąć ten produkt?' } });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        var deleteIndex = this.storeroomData.ownedGroceries.findIndex(o => o.idOwnedFoodItem == ownedGroceryId);
        this.storeroomData.ownedGroceries.splice(deleteIndex, 1);
        this.dataService.deleteStoreroomItem(ownedGroceryId);
      }
    });  
  }

  async getStoreroomItems() {
    this.storeroomData = await this.dataService.getStoreroomData();
  }

  openDialog(mode: boolean, ownedGrocery: any): void {
    const dialogRef = this.dialog.open(UserStoreroomDialogComponent, { data: { mode: mode, data: ownedGrocery } });
  }
}

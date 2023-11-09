import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AdminGroceryDialogComponent } from '../admin-grocery-dialog/admin-grocery-dialog.component';
import { Grocery } from '../interface/Grocery';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'admin-grocery-list',
  templateUrl: './admin-grocery-list.component.html',
  styleUrls: ['./admin-grocery-list.component.css']
})
export class AdminGroceryListComponent {
  displayedColumns: string[] = ['id', 'name'];
  dataSource = new MatTableDataSource<Grocery>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  constructor(public dialog: MatDialog, public adminDataService: AdminDataService) {
    this.getNumberOfGroceries();

    var pageEvent = {
      length: this.length,
      pageSize: this.pageSize,
      pageIndex: this.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  public async handlePageEvent(pageEvent: PageEvent) {
    var tags = await this.adminDataService.getGroceriesPage(pageEvent);

    this.dataSource = new MatTableDataSource<Grocery>(tags);
  }

  public async getNumberOfGroceries() {
    this.length = await this.adminDataService.getNumberOfGroceries();
  }

  newGrocery() {
    var grocery: Grocery = {
      idFoodItem: 0,
      name: ''
    };

    this.openDialog(true, grocery);
  }

  editGrocery(grocery: Grocery) {
    this.openDialog(false, grocery);
  }

  refreshTable() {
    this.getNumberOfGroceries();

    var pageEvent = {
      length: this.length,
      pageSize: this.paginator.pageSize,
      pageIndex: this.paginator.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  openDialog(mode: boolean, grocery: Grocery): void {
    const dialogRef = this.dialog.open(AdminGroceryDialogComponent, { data: { mode: mode, grocery: grocery } });
  }


}

import { Component, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AdminGroceryDialogComponent } from '../admin-grocery-dialog/admin-grocery-dialog.component';
import { Filter } from '../interface/Filter';
import { Grocery } from '../interface/Grocery';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'admin-grocery-list',
  templateUrl: './admin-grocery-list.component.html',
  styleUrls: ['./admin-grocery-list.component.css']
})
export class AdminGroceryListComponent {
  displayedColumns: string[] = ['id', 'name', 'unit'];
  dataSource = new MatTableDataSource<Grocery>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  filterForm = this.fb.group({
    name: [''],
    sort: [0],
    filtr: [0]
  });

  sortSelection = [
    { value: 1, viewValue: 'Nazwa ↑' },
    { value: 2, viewValue: 'Nazwa ↓' },
    { value: 3, viewValue: 'Miara ↑' },
    { value: 4, viewValue: 'Miara ↓' },
  ]

  constructor(public dialog: MatDialog, public adminDataService: AdminDataService, private fb: FormBuilder) {
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

    var filtr: Filter = {
      name: this.filterForm.value.name!,
      sort: this.filterForm.value.sort!,
      filtr: this.filterForm.value.filtr
    }

    if (filtr.filtr == 0) {
      filtr.filtr = []
    }

    var tags = await this.adminDataService.getGroceriesPage(pageEvent, filtr);

    this.dataSource = new MatTableDataSource<Grocery>(tags);
  }

  public async getNumberOfGroceries() {
    this.length = await this.adminDataService.getNumberOfGroceries();
  }

  newGrocery() {
    var grocery: Grocery = {
      idFoodItem: 0,
      name: '',
      unit: ''
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

  sortPage() {
    this.refreshTable();
  }

}

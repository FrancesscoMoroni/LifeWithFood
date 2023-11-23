import { Component, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { filter } from 'rxjs';
import { AdminTagDialogComponent } from '../admin-tag-dialog/admin-tag-dialog.component';
import { Filter } from '../interface/Filter';
import { Tag } from '../interface/Tag';
import { AdminDataService } from '../services/admin-data.service';
import { YesNoDialogComponent } from '../yes-no-dialog/yes-no-dialog.component';

@Component({
  selector: 'admin-tags-list',
  templateUrl: './admin-tags-list.component.html',
  styleUrls: ['./admin-tags-list.component.css']
})
export class AdminTagsListComponent {

  filterForm = this.fb.group({
    name: [''],
    sort: [0],
    filtr: [0]
  });

  sortSelection = [
    { value: 1, viewValue: 'Nazwa ↑' },
    { value: 2, viewValue: 'Nazwa ↓' },
    { value: 3, viewValue: 'Priorytet ↑' },
    { value: 4, viewValue: 'Priorytet ↓' },
  ]

  displayedColumns: string[] = ['id', 'name', 'priority', 'options'];
  dataSource = new MatTableDataSource<Tag>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  constructor(public dialog: MatDialog, public adminDataService: AdminDataService, private fb: FormBuilder) {
    this.getNumberOfTags();

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

    var tags = await this.adminDataService.getTagsPage(pageEvent, filtr);

    this.dataSource = new MatTableDataSource<Tag>(tags);
  }

  public async getNumberOfTags() {
    this.length = await this.adminDataService.getNumberOfTags();
  }

  newTag() {
    var tag: Tag = {
      idTag: 0,
      name: '',
      priority: 0
    };

    this.openDialog(true, tag);
  }

  editTag(tag: Tag) {
    this.openDialog(false, tag);
  }

  refreshTable() {
    this.getNumberOfTags();

    var pageEvent = {
      length: this.length,
      pageSize: this.paginator.pageSize,
      pageIndex: this.paginator.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  openDialog(mode: boolean, tag: Tag): void {
    const dialogRef = this.dialog.open(AdminTagDialogComponent, { data: { mode: mode, tag: tag } });
  }

  deleteTag(id: number) {
    const dialogRef = this.dialog.open(YesNoDialogComponent, { data: { title: 'usunąć ten tag?' } });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.adminDataService.deleteTag(id);
      }
    });
  }

  sortPage() {
    this.refreshTable();
  }
}

import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AdminTagDialogComponent } from '../admin-tag-dialog/admin-tag-dialog.component';
import { Tag } from '../interface/Tag';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'admin-tags-list',
  templateUrl: './admin-tags-list.component.html',
  styleUrls: ['./admin-tags-list.component.css']
})
export class AdminTagsListComponent {

  displayedColumns: string[] = ['id', 'name', 'priority'];
  dataSource = new MatTableDataSource<Tag>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  constructor(public dialog: MatDialog, public adminDataService: AdminDataService) {
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
    var tags = await this.adminDataService.getTagsPage(pageEvent);

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
}

import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AdminUserDialogComponent } from '../admin-user-dialog/admin-user-dialog.component';
import { User } from '../interface/User';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'admin-user-list',
  templateUrl: './admin-user-list.component.html',
  styleUrls: ['./admin-user-list.component.css']
})
export class AdminUserListComponent {
  displayedColumns: string[] = ['id', 'login', 'name', 'password', 'role', 'date'];
  roles: string[] = ['', 'Admin', 'Użytkownik'];
  dataSource = new MatTableDataSource<User>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  constructor(public dialog: MatDialog, public adminDataService: AdminDataService) {
    this.getNumberOfUsers();

    var pageEvent = {
      length: this.length,
      pageSize: this.pageSize,
      pageIndex: this.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  public async handlePageEvent(pageEvent: PageEvent) {
    var tags = await this.adminDataService.getUsersPage(pageEvent);

    this.dataSource = new MatTableDataSource<User>(tags);
  }

  public async getNumberOfUsers() {
    this.length = await this.adminDataService.getNumberOfUsers();
  }

  newUser() {
    var user: User = {
      idUser: 0,
      login: '',
      name: '',
      password: '',
      role: 0,
      createDate: new Date()
    };

    this.openDialog(true, user);
  }

  editUser(user: User) {
    this.openDialog(false, user);
  }

  refreshTable() {
    this.getNumberOfUsers();

    var pageEvent = {
      length: this.length,
      pageSize: this.paginator.pageSize,
      pageIndex: this.paginator.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  openDialog(mode: boolean, user: User): void {
    const dialogRef = this.dialog.open(AdminUserDialogComponent, { data: { mode: mode, user: user } });
  }
}
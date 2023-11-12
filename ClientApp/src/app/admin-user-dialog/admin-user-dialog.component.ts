import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from '../interface/User';
import { AdminDataService } from '../services/admin-data.service';

export interface DialogData {
  mode: boolean;
  user: User;
}

@Component({
  selector: 'admin-user-dialog',
  templateUrl: './admin-user-dialog.component.html',
  styleUrls: ['./admin-user-dialog.component.css']
})
export class AdminUserDialogComponent {
  okButtonName: string = 'Utwórz Nowy';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Nowy Użytkownik';

  roles = [
    { value: 1, viewValue: 'Admin' },
    { value: 2, viewValue: 'Użytkownik' }
  ];

  userForm = this.fb.group({
    login: [this.data.user.login, Validators.required],
    password: [this.data.user.password, Validators.required],
    name: [this.data.user.name, Validators.required],
    role: ['Użytkownik', Validators.required]
  });

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<AdminUserDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData, private adminDataService: AdminDataService) {
    if (data.mode) {
      this.okButtonName = 'Utwórz Nowy';
      this.title = 'Nowy Użytkownik';
    } else {
      this.okButtonName = 'Edytuj';
      this.title = 'Edytuj Użytkownika';
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    if (this.userForm.status == 'VALID') {
      if (this.data.mode) {
        this.data.user.login = this.userForm.value.login!;
        this.data.user.name = this.userForm.value.name!;
        this.data.user.role = parseInt(this.userForm.value.role!);
        this.data.user.password = this.userForm.value.password!;

        this.adminDataService.createNewUser(this.data.user);
      } else {
        this.data.user.login = this.userForm.value.login!;
        this.data.user.name = this.userForm.value.name!;
        this.data.user.role = parseInt(this.userForm.value.role!);
        this.data.user.password = this.userForm.value.password!;

        this.adminDataService.editUser(this.data.user);
      }

      this.dialogRef.close();
    }
  }
}

import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Tag } from '../interface/Tag';
import { AdminDataService } from '../services/admin-data.service';


export interface DialogData {
  mode: boolean;
  tag: Tag;
}

@Component({
  selector: 'admin-tag-dialog',
  templateUrl: './admin-tag-dialog.component.html',
  styleUrls: ['./admin-tag-dialog.component.css']
})
export class AdminTagDialogComponent {
  tagForm = this.fb.group({
    name: [this.data.tag.name, Validators.required],
    priority: [this.data.tag.priority, Validators.required],
  });

  okButtonName: string = 'Utwórz Nowy';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Nowy Tag';

  priority = [
    { value: 0, viewValue: '0' },
    { value: 1, viewValue: '1' },
    { value: 2, viewValue: '2' },
    { value: 3, viewValue: '3' },
    { value: 4, viewValue: '4' },
    { value: 5, viewValue: '5' },
    { value: 6, viewValue: '6' },
    { value: 7, viewValue: '7' },
    { value: 8, viewValue: '8' },
    { value: 9, viewValue: '9' },
    { value: 10, viewValue: '10' },
  ];

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<AdminTagDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData, private adminDataService: AdminDataService) {
    if (data.mode) {
      this.okButtonName = 'Utwórz Nowy';
      this.title = 'Nowy Tag';
    } else {
      this.okButtonName = 'Edytuj';
      this.title = 'Edytuj Tag';
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    if (this.tagForm.status == 'VALID') {
      if (this.data.mode) {
        var tag = {
          Name: this.tagForm.value.name,
          Priority: this.tagForm.value.priority
        }
        this.adminDataService.createNewTag(tag);
      } else {
        this.data.tag.name = this.tagForm.value.name!;
        this.data.tag.priority = this.tagForm.value.priority!;
        this.adminDataService.editTag(this.data.tag);
      }

      this.dialogRef.close();
    }
  }
}

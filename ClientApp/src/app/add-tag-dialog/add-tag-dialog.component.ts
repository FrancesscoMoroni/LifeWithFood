import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from '../services/data-service.service';

export interface TagData {
  ingredient: {
    grocery: Object,
    name: string,
    quantity: number
  },
  mode: boolean
}

@Component({
  selector: 'add-tag-dialog',
  templateUrl: './add-tag-dialog.component.html',
  styleUrls: ['./add-tag-dialog.component.css']
})
export class AddTagDialogComponent {

  okButtonName: string = 'Dodaj Tag';
  cancelButtonName: string = 'Anuluj';
  title: string = 'Dodaj Tag';

  tagsSelection: any;

  tagForm = this.fb.group({
    tag: ['', Validators.required]
  });

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<AddTagDialogComponent>, public dataService: DataService) {
    this.getTags();
  }

  async getTags() {
    var result = await this.dataService.getTagSelect();

    this.tagsSelection = result;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    if (this.tagForm.status == 'VALID') {
      this.dialogRef.close(this.tagForm.value);
    } 
  }
}

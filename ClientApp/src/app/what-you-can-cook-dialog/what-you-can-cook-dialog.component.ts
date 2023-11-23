import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { WhatYouCanCookFilter } from '../interface/WhatYouCanCookFilter';
import { DataService } from '../services/data-service.service';

@Component({
  selector: 'what-you-can-cook-dialog',
  templateUrl: './what-you-can-cook-dialog.component.html',
  styleUrls: ['./what-you-can-cook-dialog.component.css']
})
export class WhatYouCanCookDialogComponent {
  title = "Przepisy, na które posidasz składniki";

  filterForm = this.fb.group({
    amount: [10],
    filtr: [0]
  });

  tagsSelection = [{
    name: '',
    tag: {
      idTag: 0,
      name: '',
      priority: 0
    }
  }];

  recipes: any;

  constructor(public dialogRef: MatDialogRef<WhatYouCanCookDialogComponent>, private fb: FormBuilder, private dataService: DataService) {
    this.getTags()

    var filter: WhatYouCanCookFilter = {
      amount: 10,
      filtr: []
    }

    this.whatCanYouCook(filter);
  }

  async getTags() {
    var result = await this.dataService.getTagSelect();

    this.tagsSelection = result;
  }

  sort() {
    var filtr: WhatYouCanCookFilter = {
      amount: this.filterForm.value.amount!,
      filtr: this.filterForm.value.filtr
    }

    if (filtr.filtr == 0) {
      filtr.filtr = []
    }

    this.whatCanYouCook(filtr);
  }

  async whatCanYouCook(filter: WhatYouCanCookFilter) {
    this.recipes = await this.dataService.whatYouCanCook(filter);
  }

  onCloseClick(): void {
    this.dialogRef.close(false);
  }

}

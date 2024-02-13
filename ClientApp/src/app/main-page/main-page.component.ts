import { Component, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Filter } from '../interface/Filter';
import { DataService } from '../services/data-service.service';
import { MainPageRecipeService } from '../services/main-page-recipe.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  filterForm = this.fb.group({
    name: [''],
    sort: [0],
    filtr: [0]
  });

  sortSelection = [
    { value: 1, viewValue: 'Nazwa ↑' },
    { value: 2, viewValue: 'Nazwa ↓' },
    { value: 3, viewValue: 'Czas gotowania ↑' },
    { value: 4, viewValue: 'Czas gotowania ↓' },
    { value: 5, viewValue: 'Ocena ↑' },
    { value: 6, viewValue: 'Ocena ↓' },
    { value: 7, viewValue: 'Data dodania ↑' },
    { value: 8, viewValue: 'Data dodania ↓' },
  ];

  tagsSelection = [{
    name: '',
    tag: {
      idTag: 0,
      name: '',
      priority: 0
    }
  }];

  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  public recipes: any;

  constructor(private mainPageRecipeService: MainPageRecipeService, private fb: FormBuilder, private dataService: DataService) {
    this.getNumberOfRecipes();

    var pageEvent = {
      length: this.length,
      pageSize: this.pageSize,
      pageIndex: this.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
    this.getTags();
  }

  async getTags() {
    var result = await this.dataService.getTagSelect();

    this.tagsSelection = result;
  }

  refreshList() {
    this.getNumberOfRecipes();

    var pageEvent = {
      length: this.length,
      pageSize: this.paginator.pageSize,
      pageIndex: this.paginator.pageIndex,
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

    this.recipes = await this.mainPageRecipeService.getPage(pageEvent, filtr);
  }

  public async getNumberOfRecipes() {
    this.length = await this.mainPageRecipeService.getNumberOfRecipes();
  }

  sortPage() {
    this.refreshList();
  }
}

import { Component, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Filter } from '../interface/Filter';
import { Recipe } from '../interface/Recipe';
import { DataService } from '../services/data-service.service';
import { UserRecipeDialogComponent } from '../user-recipe-dialog/user-recipe-dialog.component';

@Component({
  selector: 'app-user-recipes-page',
  templateUrl: './user-recipes-page.component.html',
  styleUrls: ['./user-recipes-page.component.css']
})
export class UserRecipesPageComponent {
  displayedColumns: string[] = ['id', 'name', 'createDate', 'editDate'];
  dataSource = new MatTableDataSource<Recipe>();
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
  ;

  constructor(public dialog: MatDialog,  private fb: FormBuilder, private dataService: DataService) {
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

  public async handlePageEvent(pageEvent: PageEvent) {

    var filtr: Filter = {
      name: this.filterForm.value.name!,
      sort: this.filterForm.value.sort!,
      filtr: this.filterForm.value.filtr
    }

    if (filtr.filtr == 0) {
      filtr.filtr = []
    }

    var recipes = await this.dataService.getRecipesPage(pageEvent, filtr);
    this.dataSource = new MatTableDataSource<Recipe>(recipes);
  }

  public async getNumberOfRecipes() {
    this.length = await this.dataService.getNumberOfRecipes();
  }

  newRecipe() {
    var recipe: Recipe = {
      idRecipe: 0,
      name: '',
      description: '',
      instruction: '',
      prepTime: 0,
      createDate: new Date,
      editDate: new Date,
      creator: '',
      tagsIdTags: [{
        idTag: 0,
        name: '',
        priority: 0
      }],
      listsOfIngredients: [{
        name: '',
        quanity: 0,
        groceriesIdFoodItemNavigation: Object
      }]
    };

    this.openDialog(true, recipe);
  }

  editRecipe(recipe: Recipe) {
    this.openDialog(false, recipe);
  }

  refreshTable() {
    this.getNumberOfRecipes();

    var pageEvent = {
      length: this.length,
      pageSize: this.paginator.pageSize,
      pageIndex: this.paginator.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  openDialog(mode: boolean, recipe: Recipe): void {
    const dialogRef = this.dialog.open(UserRecipeDialogComponent, { data: { mode: mode, recipe: recipe } });
  }

  sortPage() {
    this.refreshTable();
  }
}

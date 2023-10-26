import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MainPageRecipeService } from '../services/main-page-recipe.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {
  private mainPageRecipeService: MainPageRecipeService;

  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  public recipes: any;

  constructor(mainPageRecipeService: MainPageRecipeService) {
    this.mainPageRecipeService = mainPageRecipeService;

    this.getNumberOfRecipes();

    var pageEvent = {
      length: this.length,
      pageSize: this.pageSize,
      pageIndex: this.pageIndex,
      previousPageIndex: 0
    }

    this.handlePageEvent(pageEvent);
  }

  public async handlePageEvent(pageEvent: PageEvent) {
    this.recipes = await this.mainPageRecipeService.getPage(pageEvent);
  }

  public async getNumberOfRecipes() {
    this.length = await this.mainPageRecipeService.getNumberOfRecipes();
  }
}

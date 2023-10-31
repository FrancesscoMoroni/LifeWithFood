import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecipePageService } from '../services/recipe-page.service';

@Component({
  selector: 'app-recipe-page',
  templateUrl: './recipe-page.component.html',
  styleUrls: ['./recipe-page.component.css']
})
export class RecipePageComponent {
  private recipePageService: RecipePageService;

  public recipeId: number;
  public recipeData = {
    'createDate': '',
    'description': '',
    'editDate': '',
    'groceriesIdFoodItems': '',
    'idRecipe': '',
    'images': '',
    'instruction': '',
    'name': '',
    'prepTime': '',
    'ratings': '',
    'tagsIdTags': '',
    'usersIdUsers': '',
  }

  constructor(activatedRoute: ActivatedRoute, recipePageService : RecipePageService) {
    this.recipeId = activatedRoute.snapshot.params['id'];
    this.recipePageService = recipePageService;

    this.getRecipe();
  };

  public async getRecipe() {
    this.recipeData = await this.recipePageService.getRecipe(this.recipeId);
    console.log(this.recipeData);
  }

}

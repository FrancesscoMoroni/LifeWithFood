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
  public favorite: boolean = false;
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

    this.ifFavorite();
    this.getRecipe();
  };

  public async ifFavorite() {
    this.favorite = await this.recipePageService.ifRecipeIsFavorite(this.recipeId);
  }

  public async getRecipe() {
    this.recipeData = await this.recipePageService.getRecipe(this.recipeId);
  }

  public async makeFavorite(id: number) {
    
    await this.recipePageService.menageFovoriteRecipe(this.recipeId);

    this.favorite = !this.favorite;
  }

}

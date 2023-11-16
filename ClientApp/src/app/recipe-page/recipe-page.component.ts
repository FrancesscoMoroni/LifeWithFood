import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RecipePageService } from '../services/recipe-page.service';

@Component({
  selector: 'app-recipe-page',
  templateUrl: './recipe-page.component.html',
  styleUrls: ['./recipe-page.component.css']
})
export class RecipePageComponent {
  public recipeId: number;
  public favorite: boolean = false;
  public recipeData = {
    'idRecipe': 0,
    'userName': '',
    'name': '',
    'description': '',
    'createDate': Date,
    'editDate': Date,
    'instruction': '',
    'prepTime': 0,
    'ratings': [{
      'idRating': 0,
      'idRecipe': 0,
      'score': 0,
      'comment': '',
      'date': Date,
      'userName': ''
    }],
    'tags': [{
      'name': '',
      'priority': 0
    }],
    'ingredients': [{
      'name': '',
      'quantity': '',
      'unit': ''
    }],
    'finalScore': 0
  }

  ratingForm = this.fb.group({
    comment: ['', Validators.required],
    score: [1, Validators.required],
  });

  rating = [
    { value: 1, viewValue: '1' },
    { value: 2, viewValue: '2' },
    { value: 3, viewValue: '3' },
    { value: 4, viewValue: '4' },
    { value: 5, viewValue: '5' }
  ];

  constructor(activatedRoute: ActivatedRoute,private recipePageService: RecipePageService, private fb: FormBuilder) {
    this.recipeId = activatedRoute.snapshot.params['id'];

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

  public async addComment() {
    let newRating = {
      idRecipe: this.recipeData.idRecipe,
      comment: this.ratingForm.value.comment,
      score: this.ratingForm.value.score
    }

    if (this.ratingForm.status == "VALID") {
      await this.recipePageService.addNewRating(newRating);
    }
  }

  public async checkOnwedIngredients() {
    var ingredients = await this.recipePageService.checkOwnedIngredients(this.recipeId);

    if (ingredients?.length != 0) {
      const blob = new Blob([JSON.stringify(ingredients)], { type: 'txt' });
      let a = document.createElement('a');
      a.download = "Składniki na " + this.recipeData.name;
      a.href = window.URL.createObjectURL(blob);

      a.click();
    }
  }

}

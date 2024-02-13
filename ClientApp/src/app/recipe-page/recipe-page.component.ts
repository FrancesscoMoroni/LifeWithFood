import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { RecipePageService } from '../services/recipe-page.service';
import { UserAuthService } from '../services/user-auth.service';
import { ShoppingListDialogComponent } from '../shopping-list-dialog/shopping-list-dialog.component';

@Component({
  selector: 'app-recipe-page',
  templateUrl: './recipe-page.component.html',
  styleUrls: ['./recipe-page.component.css']
})
export class RecipePageComponent {
  public iconClass = "icon-white";
  public role = 0;
  public idRecipe: number;
  public favorite: boolean = false;
  public recipeData = {
    'idRecipe': 0,
    'creatorName': '',
    'name': '',
    'description': '',
    'createDate': new Date(),
    'editDate': new Date(),
    'instruction': '',
    'prepTime': 0,
    'ratings': [{
      'idRating': 0,
      'idRecipe': 0,
      'score': 0,
      'comment': '',
      'date': new Date(),
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

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private recipePageService: RecipePageService, private fb: FormBuilder, public dialog: MatDialog, private userAuthService: UserAuthService) {
    this.idRecipe = activatedRoute.snapshot.params['id'];
    this.constructorAsync();

    router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        this.checkRole();
      }
    });
  };

  async constructorAsync() {
    await this.checkRole();
    await this.ifFavorite();
    await this.getRecipe();
  }

  async checkRole() {
    this.role = await this.userAuthService.checkRole();
  }

  public async ifFavorite() {
    if (this.role != 0) {
      this.favorite = await this.recipePageService.ifRecipeIsFavorite(this.idRecipe);

      if (!this.favorite) {
        this.iconClass = "icon-white";
      } else {
        this.iconClass = "icon-red";
      }
    }
  }

  public async getRecipe() {
    this.recipeData = await this.recipePageService.getRecipe(this.idRecipe);
  }

  public async makeFavorite(id: number) {
    
    await this.recipePageService.menageFovoriteRecipe(this.idRecipe);

    this.favorite = !this.favorite;

    if (!this.favorite) {
      this.iconClass = "icon-white";
    } else {
      this.iconClass = "icon-red";
    }
  }

  public async addComment() {
    let newRating = {
      idRecipe: this.recipeData.idRecipe,
      comment: this.ratingForm.value.comment,
      score: this.ratingForm.value.score
    }

    if (this.ratingForm.status == "VALID") {
      let anwser = await this.recipePageService.addNewRating(newRating);
      this.recipeData.ratings.push({
        'idRating': 0,
        'idRecipe': 0,
        'score': Number(newRating.score),
        'comment': newRating.comment!,
        'date': new Date(),
        'userName': anwser.name
      });
      this.ratingForm.setValue({
        comment: "",
        score: 0,
      });
    }
  }

  public async checkOnwedIngredients() {
    const dialogRef = this.dialog.open(ShoppingListDialogComponent, { data: { idRecipe: this.idRecipe, name: this.recipeData.name } });
  }

}

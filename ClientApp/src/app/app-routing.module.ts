import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { MainPageComponent } from './main-page/main-page.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { UserStoreroomPageComponent } from './user-storeroom-page/user-storeroom-page.component';
import { UserRecipesPageComponent } from './user-recipes-page/user-recipes-page.component';
import { UserFavoriteRecipesPageComponent } from './user-favorite-recipes-page/user-favorite-recipes-page.component';
import { RecipePageComponent } from './recipe-page/recipe-page.component';
import { userGuardGuard } from './guards/user-guard.guard';

const appRout: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login-page', component: LoginPageComponent },
  { path: 'register-page', component: RegisterPageComponent, canActivate: [userGuardGuard], },
  { path: 'main-page', component: MainPageComponent },
  { path: 'recipe-page', component: RecipePageComponent },
  { path: 'user-favorite-recipes-page', component: UserFavoriteRecipesPageComponent },
  { path: 'user-recipes-page', component: UserRecipesPageComponent },
  { path: 'user-storeroom-page', component: UserStoreroomPageComponent },
  { path: '**', component: PageNotFoundComponent},
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(appRout)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { MainPageComponent } from './main-page/main-page.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { UserStoreroomPageComponent } from './user-storeroom-page/user-storeroom-page.component';
import { UserRecipesPageComponent } from './user-recipes-page/user-recipes-page.component';
import { UserFavoriteRecipesPageComponent } from './user-favorite-recipes-page/user-favorite-recipes-page.component';
import { RecipePageComponent } from './recipe-page/recipe-page.component';
import { AdminPageComponent } from './admin-page/admin-page.component';

import { adminGuardGuard } from './guards/admin-guard.guard';
import { userGuardGuard } from './guards/user-guard.guard';

const appRout: Routes = [
  { path: '', component: MainPageComponent, pathMatch: 'full' },
  { path: 'login-page', component: LoginPageComponent },
  { path: 'register-page', component: RegisterPageComponent},
  { path: 'main-page', component: MainPageComponent },
  { path: 'recipe-page', component: RecipePageComponent },
  { path: 'user-favorite-recipes-page', component: UserFavoriteRecipesPageComponent, canActivate: [userGuardGuard]},
  { path: 'user-recipes-page', component: UserRecipesPageComponent, canActivate: [userGuardGuard]},
  { path: 'user-storeroom-page', component: UserStoreroomPageComponent, canActivate: [userGuardGuard] },
  { path: 'admin-page', component: AdminPageComponent, canActivate: [adminGuardGuard] },
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

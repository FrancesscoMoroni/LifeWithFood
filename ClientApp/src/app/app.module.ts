import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { MatCardModule } from '@angular/material/card';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatChipsModule } from '@angular/material/chips';

import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { MainPageComponent } from './main-page/main-page.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ProfilePageComponent } from './profile-page/profile-page.component';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { RecipePageComponent } from './recipe-page/recipe-page.component';
import { UserRecipesPageComponent } from './user-recipes-page/user-recipes-page.component';
import { UserStoreroomPageComponent } from './user-storeroom-page/user-storeroom-page.component';
import { UserFavoriteRecipesPageComponent } from './user-favorite-recipes-page/user-favorite-recipes-page.component';
import { SideOptionsComponent } from './side-options/side-options.component';
import { StructureComponent } from './structure/structure.component';
import { AdminRecipesListComponent } from './admin-recipes-list/admin-recipes-list.component';
import { AdminTagsListComponent } from './admin-tags-list/admin-tags-list.component';
import { AdminGroceryListComponent } from './admin-grocery-list/admin-grocery-list.component';
import { AdminUserListComponent } from './admin-user-list/admin-user-list.component';
import { AdminRecipeDialogComponent } from './admin-recipe-dialog/admin-recipe-dialog.component';
import { AdminTagDialogComponent } from './admin-tag-dialog/admin-tag-dialog.component';
import { AdminUserDialogComponent } from './admin-user-dialog/admin-user-dialog.component';
import { AdminGroceryDialogComponent } from './admin-grocery-dialog/admin-grocery-dialog.component';
import { AddIngredientDialogComponent } from './add-ingredient-dialog/add-ingredient-dialog.component';
import { AddTagDialogComponent } from './add-tag-dialog/add-tag-dialog.component';
import { UserRecipeDialogComponent } from './user-recipe-dialog/user-recipe-dialog.component';
import { UserStoreroomDialogComponent } from './user-storeroom-dialog/user-storeroom-dialog.component';
import { YesNoDialogComponent } from './yes-no-dialog/yes-no-dialog.component';
import { WhatYouCanCookDialogComponent } from './what-you-can-cook-dialog/what-you-can-cook-dialog.component';
import { ShoppingListDialogComponent } from './shopping-list-dialog/shopping-list-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    LoginPageComponent,
    RegisterPageComponent,
    MainPageComponent,
    PageNotFoundComponent,
    ProfilePageComponent,
    AdminPageComponent,
    RecipePageComponent,
    UserRecipesPageComponent,
    UserStoreroomPageComponent,
    UserFavoriteRecipesPageComponent,
    SideOptionsComponent,
    StructureComponent,
    AdminRecipesListComponent,
    AdminTagsListComponent,
    AdminGroceryListComponent,
    AdminUserListComponent,
    AdminRecipeDialogComponent,
    AdminTagDialogComponent,
    AdminUserDialogComponent,
    AdminGroceryDialogComponent,
    AddIngredientDialogComponent,
    AddTagDialogComponent,
    UserRecipeDialogComponent,
    UserStoreroomDialogComponent,
    YesNoDialogComponent,
    WhatYouCanCookDialogComponent,
    ShoppingListDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatPaginatorModule,
    MatCardModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatTabsModule,
    MatTableModule,
    MatDialogModule,
    MatSelectModule,
    MatDividerModule,
    MatListModule,
    MatExpansionModule,
    MatChipsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

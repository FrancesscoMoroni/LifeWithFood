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
    StructureComponent
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
    MatTabsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

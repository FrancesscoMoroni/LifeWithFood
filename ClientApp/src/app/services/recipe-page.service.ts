import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RecipePageService {
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  public async getRecipe(id: any) {
    var page: any;

    page = await this.http.post<any>(this.baseUrl + "recipepage/getrecipe", { 'Id': id }).toPromise();
    return page;
  }

  public async menageFovoriteRecipe(recipeId: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/menagefovoriterecipe", { id: recipeId }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async ifRecipeIsFavorite(recipeId: number) {
    var isFavorite: boolean;

    isFavorite = await this.http.post<any>(this.baseUrl + "recipepage/iffavoriterecipe", { id: recipeId }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return isFavorite;
  }

  public async addNewRating(newRating: any) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/addnewrating", newRating, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async deleteRating(recipeId: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/deleterating", { id: recipeId }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async getRatings(recipeId: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/getratings", { id: recipeId }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async checkOwnedIngredients(recipeId: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/checkownedingredients", { id: recipeId }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  private getJwt() {
    return localStorage.getItem('jwt') || '';
  }

}

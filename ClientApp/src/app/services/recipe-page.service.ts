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

  public async menageFovoriteRecipe(idRecipe: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/menagefovoriterecipe", { id: idRecipe }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async ifRecipeIsFavorite(idRecipe: number) {
    var isFavorite: boolean;

    isFavorite = await this.http.post<any>(this.baseUrl + "recipepage/iffavoriterecipe", { id: idRecipe }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return isFavorite;
  }

  public async addNewRating(newRating: any) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/addnewrating", newRating, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async deleteRating(idRecipe: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/deleterating", { id: idRecipe }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async getRatings(idRecipe: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/getratings", { id: idRecipe }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  public async checkOwnedIngredients(idRecipe: number) {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "recipepage/checkownedingredients", { id: idRecipe }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return answear;
  }

  private getJwt() {
    return localStorage.getItem('jwt') || '';
  }

}

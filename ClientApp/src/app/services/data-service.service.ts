import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Filter } from '../interface/Filter';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  public async getIngredientsSelect() {
    var page: any;

    page = await this.http.get<any>(this.baseUrl + "data/getingredientsselect", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getRecipesPage(pageEvent: PageEvent, filter: Filter) {
    var page: any;

    var pageSetting = {
      length: pageEvent.length,
      pageIndex: pageEvent.pageIndex,
      pageSize: pageEvent.pageSize,
      previousPageIndex: 0,
      name: filter.name,
      sort: filter.sort,
      filtr: filter.filtr
    }

    page = await this.http.post<any>(this.baseUrl + "data/getrecipes", pageSetting, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getNumberOfRecipes() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "admindata/getnumberofrecipes", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
  }

  public async getTagSelect() {
    var page: any;

    page = await this.http.get<any>(this.baseUrl + "data/gettagselect", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async createNewRecipe(recipe: any): Promise<string> {
    var answear: any;

    recipe.jwt = this.getJwt();

    answear = await this.http.post<any>(this.baseUrl + "data/createnewrecipe", recipe, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async editRecipe(recipe: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "data/editrecipe", recipe, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  private getJwt() {
    return localStorage.getItem('jwt') || '';
  }
}

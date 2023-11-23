import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Filter } from '../interface/Filter';
import { Tag } from '../interface/Tag';

@Injectable({
  providedIn: 'root'
})
export class AdminDataService {
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  //Recipes

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

    page = await this.http.post<any>(this.baseUrl + "admindata/getrecipes", pageSetting, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getNumberOfRecipes() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "admindata/getnumberofrecipes", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
  }

  public async createNewRecipe(recipe: any): Promise<string> {
    var answear: any;

    recipe.jwt = this.getJwt();

    answear = await this.http.post<any>(this.baseUrl + "admindata/createnewrecipe", recipe, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async editRecipe(recipe: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/editrecipe", recipe, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async deleteRecipe(id: number): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/deleterecipe", {Id: id} , { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  //Groceries

  public async getGroceriesPage(pageEvent: PageEvent, filter: Filter) {
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

    page = await this.http.post<any>(this.baseUrl + "admindata/getgroceries", pageSetting, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getNumberOfGroceries() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "admindata/getnumberofgroceries", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
  }

  public async createNewGrocery(grocery: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/createnewgrocery", grocery, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async editGrocery(grocery: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/editgrocery", grocery, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async deleteGrocery(id: number): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/deletegrocery", { Id: id }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  //Tag

  public async getTagsPage(pageEvent: PageEvent, filter: Filter) {
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

    page = await this.http.post<any>(this.baseUrl + "admindata/gettags", pageSetting, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getNumberOfTags() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "admindata/getnumberoftags", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
  }

  public async createNewTag(tag: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/createnewtag", tag, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async editTag(tag: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/edittag", tag, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async deleteTag(id: number): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/deletetag", { Id: id }, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  //User

  public async getUsersPage(pageEvent: PageEvent, filter: Filter) {
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

    page = await this.http.post<any>(this.baseUrl + "admindata/getusers", pageSetting, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getNumberOfUsers() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "admindata/getnumberofusers", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
  }

  public async createNewUser(user: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/createnewuser", user, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async editUser(user: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "admindata/edituser", user, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  private getJwt() {
    return localStorage.getItem('jwt') || '';
  }
}

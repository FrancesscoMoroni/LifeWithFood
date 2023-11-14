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

  public async getTagSelect() {
    var page: any;

    page = await this.http.get<any>(this.baseUrl + "data/gettagselect", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getFavoriteRecipesPage(pageEvent: PageEvent, filter: Filter) {
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

    page = await this.http.post<any>(this.baseUrl + "data/getfavoriterecipes", pageSetting, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return page;
  }

  public async getNumberOfFavoriteRecipes() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "data/getnumberoffavoriterecipes", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
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

    number = await this.http.get<any>(this.baseUrl + "data/getnumberofrecipes", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return number;
  }

  public async createNewRecipe(recipe: any): Promise<string> {
    var answear: any;

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

  public async getStoreroomData() {
    var data: any;

    data = await this.http.get<any>(this.baseUrl + "data/getstoreroomitems", { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return data;
  }

  public async deleteStoreroomItem(id: any) {
    var storeroomItemDelete: any;

    storeroomItemDelete = await this.http.post<any>(this.baseUrl + "data/deletestoreroomitem", {id: id}, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();
    return storeroomItemDelete;
  }

  public async createNewStoreromeItem(storeroomItem: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "data/createnewstoreroomitem", storeroomItem, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  public async editStoreromeItem(storeroomItem: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "data/editstoreroomitem", storeroomItem, { headers: { 'authorization': 'bearer ' + this.getJwt() } }).toPromise();

    if (answear !== '') {
      return answear;
    }

    return '';
  }

  private getJwt() {
    return localStorage.getItem('jwt') || '';
  }
}

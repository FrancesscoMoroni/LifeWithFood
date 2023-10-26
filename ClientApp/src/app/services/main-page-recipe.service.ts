import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

@Injectable({
  providedIn: 'root'
})
export class MainPageRecipeService {
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  public async getPage(pageEvent: PageEvent) {
    var page: any;

    page = await this.http.post<any>(this.baseUrl + "mainpage/getpage", pageEvent).toPromise();
    return page;
  }

  public async getNumberOfRecipes() {
    var number: number;

    number = await this.http.get<any>(this.baseUrl + "mainpage/getnumberofrecipes").toPromise();
    return number;
  }


}

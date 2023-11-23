import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  public async loginUser(userData: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "userauthentication/userlogin", userData).toPromise();

    if (answear.error !== null) {
      return answear.error;
    }

    localStorage.setItem("jwt", answear.jwt);
    return '';
  }

  public async registerUser(userData: any): Promise<string> {
    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "userauthentication/userregister", userData).toPromise();

    if (answear.error !== null) {
      return answear.error;
    }

    localStorage.setItem("jwt", answear.jwt);
    return '';
  }

  public async userAuthorization(role: string) {
    var jwt = localStorage.getItem('jwt') || '';

    var authDto = {
      'Role': role
    }

    if (jwt == '') {
      return false;
    }

    var answear: any;

    answear = await this.http.post<any>(this.baseUrl + "userauthentication/userauth", authDto, { headers: { 'authorization': 'bearer ' + jwt} }).toPromise();

    return answear;
  }

  public async checkRole() {
    for (let i = 1; i <= 2; i++) {
      let hasRole = await this.userAuthorization(i.toString());

      if (hasRole) {
        return i;
      }
    }

    return 0;
  }

  public logout() {
    localStorage.removeItem("jwt");
  }


}

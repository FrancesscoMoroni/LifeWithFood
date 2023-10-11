import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  private http: HttpClient;
  private baseUrl: string;

  loginForm = this.formBuilder.group({
    login: '',
    password: ''
  });

  constructor(private formBuilder: FormBuilder, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  } 

  onSubmit(): void {
    console.warn('Your order has been submitted', this.loginForm.value);
      this.http.post<any>(this.baseUrl + "userauthentication/userlogin", "").subscribe(data => {
    });
  }
}

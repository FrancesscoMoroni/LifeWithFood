import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  private http: HttpClient;
  private baseUrl: string;

  public error: string = "";
  public test: boolean = true;

  registerForm = this.formBuilder.group({
    login: '',
    password: ''
  });

  constructor(private formBuilder: FormBuilder, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  onSubmit(): void {
    console.warn('Your order has been submitted', this.registerForm.value);
    this.http.post<any>(this.baseUrl + "userauthentication/userregister", this.registerForm.value).subscribe(data => {
      console.log(data);
      if (data.error !== null) {
        this.error = data.error;
        return
      }
      localStorage.setItem("jwt", data.jwt);
    });
  }
}

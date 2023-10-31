import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { UserAuthService } from '../services/user-auth.service';
import { User } from 'oidc-client';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  private userAuthService: UserAuthService;

  public error: string = "";
  public test: boolean = true;

  loginForm = this.formBuilder.group({
    login: '',
    password: ''
  });

  constructor(private formBuilder: FormBuilder, userAuthService: UserAuthService) {
    this.userAuthService = userAuthService;
  } 

  async onSubmit() {
    this.error = await Promise.resolve(this.userAuthService.loginUser(this.loginForm.value));
  }
}

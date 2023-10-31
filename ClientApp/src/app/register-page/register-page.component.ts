import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { UserAuthService } from '../services/user-auth.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  private userAuthService: UserAuthService;

  public error: string = "";
  public test: boolean = true;

  registerForm = this.formBuilder.group({
    login: '',
    password: ''
  });

  constructor(private formBuilder: FormBuilder, userAuthService: UserAuthService) {
    this.userAuthService = userAuthService;
  }

  async onSubmit() {
    this.error = await Promise.resolve(this.userAuthService.registerUser(this.registerForm.value));
  }
}

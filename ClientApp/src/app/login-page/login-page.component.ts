import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UserAuthService } from '../services/user-auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  public error: string = "";
  public test: boolean = true;

  loginForm = this.fb.group({
    login: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(private router: Router, private fb: FormBuilder, private userAuthService: UserAuthService) {} 

  async onSubmit() {
    if (this.loginForm.status == "VALID") {
      this.error = await Promise.resolve(this.userAuthService.loginUser(this.loginForm.value));
      if (this.error == '') {
        this.router.navigateByUrl('/');
      }
    }
  }
}

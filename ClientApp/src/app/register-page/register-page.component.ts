import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserAuthService } from '../services/user-auth.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  public error: string = "";
  public test: boolean = true;

  registerForm = this.fb.group({
    login: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(private router: Router, private fb: FormBuilder, private userAuthService: UserAuthService) {}

  async onSubmit() {
    if (this.registerForm.status == "VALID") {
      this.error = await Promise.resolve(this.userAuthService.registerUser(this.registerForm.value));
      if (this.error == '') {
        this.router.navigateByUrl('/');
      }
    }
  }
}

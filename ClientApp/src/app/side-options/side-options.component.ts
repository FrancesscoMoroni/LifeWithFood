import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { DrawerService } from '../services/drawer.service';
import { UserAuthService } from '../services/user-auth.service';

@Component({
  selector: 'app-side-options',
  templateUrl: './side-options.component.html',
  styleUrls: ['./side-options.component.css']
})
export class SideOptionsComponent {
  public role = 0;

  constructor(private drawerService: DrawerService, private userAuthService: UserAuthService, private router: Router) {
    this.checkRole();

    router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        this.checkRole();
      }
    });
  }

  async checkRole() {
    this.role = await this.userAuthService.checkRole();
  }

  toggle() {
    this.drawerService.toggle();
  }

  logout() {
    this.userAuthService.logout();
  }

  goTo(path: string) {
    this.toggle();
    this.checkRole();
    this.router.navigateByUrl(path);
  }

}

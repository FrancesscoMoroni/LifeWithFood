import { Component } from '@angular/core';
import { DrawerService } from '../services/drawer.service';
import { UserAuthService } from '../services/user-auth.service';

@Component({
  selector: 'app-side-options',
  templateUrl: './side-options.component.html',
  styleUrls: ['./side-options.component.css']
})
export class SideOptionsComponent {
  private drawerService: DrawerService;
  private userAuthService: UserAuthService;

  constructor(drawerService: DrawerService, userAuthService: UserAuthService) {
    this.drawerService = drawerService;
    this.userAuthService = userAuthService;
  }

  toggle() {
    this.drawerService.toggle();
  }

  logout() {
    this.userAuthService.logout();
  }

}

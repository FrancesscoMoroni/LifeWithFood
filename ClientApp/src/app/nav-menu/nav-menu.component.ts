import { Component } from '@angular/core';
import { DrawerService } from '../services/drawer.service';
import { UserAuthService } from '../services/user-auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(private drawerService: DrawerService) {}

  toggle() {
    this.drawerService.toggle();
  }
}

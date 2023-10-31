import { Component } from '@angular/core';
import { DrawerService } from '../services/drawer.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  private drawerService: DrawerService;

  constructor(drawerService: DrawerService) {
    this.drawerService = drawerService;
  }

  toggle() {
    this.drawerService.toggle();
  }
}

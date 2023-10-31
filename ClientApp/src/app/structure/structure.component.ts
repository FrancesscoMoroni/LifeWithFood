import { Component, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { DrawerService } from '../services/drawer.service';

@Component({
  selector: 'app-structure',
  templateUrl: './structure.component.html',
  styleUrls: ['./structure.component.css']
})
export class StructureComponent {
  private drawerService: DrawerService;

  @ViewChild('drawer') public drawer!: MatDrawer;

  ngAfterViewInit() {
    this.drawerService.setDrawer(this.drawer);
  }

  constructor(drawerService: DrawerService) {
    this.drawerService = drawerService;
  }
  
}

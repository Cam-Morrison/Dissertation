import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { navigationData } from './app-navigation.data';
import { fadeAnimation } from './shared/component/routing-animation';
import { AuthGuard } from './shared/services/auth.guard';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [fadeAnimation],
})
export class AppComponent {
  public navData = navigationData;
  userRole = 'Administrator';
  userName = 'Cameron Morrison';
  public innerWidth: any;
  mobile = false;
  loggedIn = false;

  constructor(private router: Router, private auth: AuthGuard) {
    router.events.subscribe((val) => {
      if(router.url == "/login")
      {
        this.loggedIn = false;
      } else {
        this.loggedIn = true;
      }

    }); 
  }

  ngOnInit() {
    this.innerWidth = window.innerWidth;
    if(innerWidth <= 1200) {
      this.mobile = true;
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.innerWidth = window.innerWidth;
    if(innerWidth <= 1200) {
      this.mobile = true;
    } else {
      this.mobile = false;
    }
  }

  @ViewChild('sidenav') ref!: MatSidenav; 
  sideNavToggle(){
    if(this.mobile == true) {
      this.ref!!.toggle();
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}

import { Component, HostListener, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { navigationData } from './app-navigation.data';
import { AuthGuard } from './shared/services/auth.guard';
import { MyDataService } from './shared/services/data.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public navData = navigationData;
  userRole = 'User';
  userName = '';
  public innerWidth: any;
  mobile = false;
  loggedIn = false;

  constructor(
    private router: Router, 
    private auth: AuthGuard,
    private myDataService: MyDataService
    ) {
    router.events.subscribe((val) => {
      if(router.url == "/login")
      {
        this.loggedIn = false;
      } else {
        this.loggedIn = true;
        try{
          var userObj = auth.getDecodedToken();
          this.userName = userObj.user;
          this.userRole = userObj.role;
        }catch(Exception){}
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

  async logout() {
    await this.myDataService.logSignOut();
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}

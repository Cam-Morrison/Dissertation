import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { navigationData } from "./app-navigation.data";
import { fadeAnimation } from "./shared/component/routing-animation";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  animations: [ fadeAnimation ]
})
export class AppComponent {

  public navData = navigationData;
  userRole = "Administrator";
  userName = "Cameron Morrison";

  constructor(private router : Router) {}
  
  scrollToTop(){
    window.scroll(0,0);
  }

  logout(){
    localStorage.removeItem('token');
    this.router.navigate(["/login"]);
  }
}

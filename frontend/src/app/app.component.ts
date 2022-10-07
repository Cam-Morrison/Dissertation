import { Component } from "@angular/core";
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

  constructor() {}
  
  scrollToTop(){
    window.scroll(0,0);
  }
}

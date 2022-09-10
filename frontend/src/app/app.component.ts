import { Component } from "@angular/core";
import { ChildrenOutletContexts } from "@angular/router";
import { navigationData } from "./app-navigation.data";
import { fadeAnimation } from "./shared/services/routing-animation";

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

  constructor(private contexts: ChildrenOutletContexts) {}
  
  scrollToTop(){
    window.scroll(0,0);
  }
}

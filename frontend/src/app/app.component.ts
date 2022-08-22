import { ThisReceiver } from "@angular/compiler";
import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { navigationData } from "./app-navigation.data";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {

  public navData = navigationData;
  userRole = "Administrator";
  userName = "Cameron Morrison";

  ngOnInit():void {
  }

  constructor() {
  }
}

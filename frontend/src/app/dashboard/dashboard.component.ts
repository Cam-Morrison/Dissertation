import { MyDataService } from '../shared/services/data.service';
import { Component, OnInit, AfterViewInit } from "@angular/core";
import { shareReplay } from 'rxjs/operators';

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.css"]
})

export class DashboardComponent implements OnInit, AfterViewInit {

  stocks: any[] = [];
  watchlistTitle: string = "";
  isLoading = true;
  private sub: any;

  constructor(private MyDataService: MyDataService) {}
  
  ngOnInit():void {
    // Read in ticker, get its price datapoints and daily movement
    let resp = this.MyDataService.getWatchList().pipe(shareReplay());
    this.sub =  resp.subscribe((data: any)=> {
      console.log(data)
      this.watchlistTitle = data["title"];
      this.stocks = data["stocks"];
      console.log(this.stocks);
      this.isLoading = false;
    },   
    (error) => {console.log(error)});  
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  ngAfterViewInit() {
  }
}

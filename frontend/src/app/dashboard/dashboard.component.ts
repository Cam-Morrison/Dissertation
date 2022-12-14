import { MyDataService } from '../shared/services/data.service';
import { Component, OnInit, AfterViewInit } from "@angular/core";
import { shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"]
})

export class DashboardComponent implements OnInit, AfterViewInit {

  stocks: any[] = [];
  watchlistTitle: string = "";
  public chartTitle: string = "Portfolio movement";
  isLoading = true;
  private sub: any;

  dailyMovement: number = 0;
  ticker?: string | null;
  tickerValid: boolean = true;
  dataPoints: any = [];
  selectedChart: any = 'area';
  previousCloses: any[] = [];
  currentPrices: any[] = [];

  public editmode: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public editmodeObs: Observable<boolean> = this.editmode.asObservable();

  constructor(private MyDataService: MyDataService) {}
  
  ngOnInit():void {
    // Read in ticker, get its price datapoints and daily movement
    let resp = this.MyDataService.getWatchList().pipe(shareReplay());
    this.sub =  resp.subscribe((data: any)=> {
      console.log(data)
      this.watchlistTitle = data["title"];
      this.stocks = data["stocks"];
      console.log(this.stocks);

      for (let i = 0; i < this.stocks.length; i++) {
        this.dailyMovement += this.stocks[i]["regularMarketChangePercent"];
        this.previousCloses.push(this.stocks[i]["regularMarketPreviousClose"])
        this.currentPrices.push(this.stocks[i]["regularMarketPrice"])

      }

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

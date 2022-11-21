import { MyDataService } from '../shared/services/data.service';
import { Component, OnInit, AfterViewInit, ViewChild } from "@angular/core";
import { map, shareReplay } from 'rxjs/operators';
import { interval } from 'rxjs/internal/observable/interval';

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.css"]
})

export class DashboardComponent implements OnInit, AfterViewInit {

  dataPoints:any = [];
  ticker = "VOO";
  tickerTitle = "S&P 500";
  showFiller = false;
  isLoading = true;
  dailyMovement: any;
  myChartService:any;
  selectedChart: string = "area";
  private sub: any;

  ngOnInit():void {
    //Read in ticker, get its price datapoints and daily movement
    let resp = this.MyDataService.getStockHistory(this.ticker).pipe(shareReplay());
    this.sub =  resp.subscribe((data: any)=> {
        for (var key in data['results']) {
          var dt = data['results'][key];
          this.dataPoints.push([[new Date(dt["t"])],
          [Number(dt["o"]), Number(dt["h"]), Number(dt["l"]), Number(dt["c"])]]);
        }
        this.isLoading = false;
        var today = this.dataPoints[this.dataPoints.length-1][1];
        var closeValue = today[3];
        var openValue = today[1];
        this.dailyMovement = (((closeValue - openValue) / openValue) * 100).toFixed(2);
    },   
    (error) => {});  
  }

  constructor(private MyDataService: MyDataService) {}
  

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  ngAfterViewInit() {
  }
}

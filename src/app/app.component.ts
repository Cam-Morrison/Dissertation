import { MyDataService } from './services/data.service';
import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { shareReplay } from 'rxjs/operators';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexYAxis,
  ApexXAxis,
  ApexTitleSubtitle
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  title: ApexTitleSubtitle;
};

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit, AfterViewInit {


  public chartOptions?: Partial<ChartOptions>;
  dataPoints:any = [];
  ticker = "PYPL";

  ngOnInit():void {
  }

  constructor(private MyDataService: MyDataService) {

    let resp = this.MyDataService.getPrices(this.ticker).pipe(shareReplay());
    resp.subscribe((data: any)=> {
        for (var key in data['Time Series (Daily)']) {
          var dt = data['Time Series (Daily)'][key];

          this.dataPoints.push({x: new Date(key), y: Number(dt["4. close"]) });
        }
        this.intializationChart();
    },
    (error) => {
      console.log("error is: "+ error);
    });

  }

  intializationChart() {

    this.chartOptions = {
      series: [{
        name: "Price",
        data: this.dataPoints
      }],
      chart: {
        id: 'chart',
        type: "line",
        height: 350
      },
      title: {
        text: `${this.ticker}`,
        align: "left"
      },
      xaxis: {
        type: "datetime"
      },
      yaxis: {
        tooltip: {
          enabled: true
        }
      }
    };
  }

  ngAfterViewInit() {
  }

}

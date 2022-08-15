import { MyDataService } from '../shared/services/data.service';
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
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.css"]
})
export class DashboardComponent implements OnInit, AfterViewInit {


  public chartOptions?: Partial<ChartOptions>;
  dataPoints:any = [];
  volume:any = [];
  ticker = "VOO";
  tickerTitle = "Vanguard S&P 500 Index";
  showFiller = false;

  ngOnInit():void {
  }

  constructor(private MyDataService: MyDataService) {

    //Read in ticker
    let resp = this.MyDataService.getPrices(this.ticker, "2017-01-01").pipe(shareReplay());
    resp.subscribe((data: any)=> {
        for (var key in data['results']) {
          var dt = data['results'][key];
          this.dataPoints.push([[new Date(dt["t"])],
          [Number(dt["o"]), Number(dt["h"]), Number(dt["l"]), Number(dt["c"])]]);
          this.volume.push(Number(dt["v"]));
        }
        this.intializationChart();
    },
    (error) => {
      console.log("error is: " + error);
    });

  }

  intializationChart() {
    this.chartOptions = {
      series: [{
        name: "Price",
        type: "candlestick",
        data: this.dataPoints
      }
    ],
      chart: {
        id: 'chart',
        type: "candlestick",
        height: 350
      },
      title: {
        text: `${this.tickerTitle}`,
        align: "center"
      },
      xaxis: {
        type: "datetime",
        axisTicks: {
          show: true,
          borderType: 'solid',
          color: '#78909C',
          height: 6,
          offsetX: 0,
          offsetY: 0
        },
        labels: {
          format: 'MM/yy',
        }
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

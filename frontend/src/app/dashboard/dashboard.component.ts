import { MyDataService } from '../shared/services/data.service';
import { Component, OnInit, AfterViewInit } from "@angular/core";
import { shareReplay } from 'rxjs/operators';
import {
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
  ticker = "VOO";
  tickerTitle = "S&P 500 Index";
  showFiller = false;

  ngOnInit():void {
    //Read in ticker
    let resp = this.MyDataService.getStockPrice(this.ticker, "2017-01-01").pipe(shareReplay());
    resp.subscribe((data: any)=> {
        for (var key in data['results']) {
          var dt = data['results'][key];
          this.dataPoints.push([[new Date(dt["t"])],
          [Number(dt["o"]), Number(dt["h"]), Number(dt["l"]), Number(dt["c"])]]);
        }
        this.intializationChart();
    },
    (error) => {
      console.log("error is: " + error);
    });
  }

  constructor(private MyDataService: MyDataService) {}

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

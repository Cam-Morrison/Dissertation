import { MyDataService } from '../shared/services/data.service';
import { Component, OnInit, AfterViewInit, ViewChild } from "@angular/core";
import { shareReplay } from 'rxjs/operators';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexYAxis,
  ApexXAxis,
  ApexTitleSubtitle,
  ApexFill,
  ApexMarkers,
  ApexTooltip,
  ApexDataLabels,
  ApexStroke,
  ChartComponent
} from "ng-apexcharts";

export type ChartOptions = {
   series: ApexAxisChartSeries;
   chart: ApexChart;
   dataLabels: ApexDataLabels;
   markers: ApexMarkers;
   title: ApexTitleSubtitle;
   fill: ApexFill;
   yaxis: ApexYAxis;
   xaxis: ApexXAxis;
   tooltip: ApexTooltip;
   stroke: ApexStroke;
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
  tickerTitle = "S&P 500";
  showFiller = false;
  isLoading = true;
  selectedChart: any = 'area';
  dailyMovement: any;

  ngOnInit():void {
    //Read in ticker
    let resp = this.MyDataService.getStockHistory(this.ticker).pipe(shareReplay());
    resp.subscribe((data: any)=> {
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
        this.initAreaChart();
    },   
    (error) => {
      console.log("error is: " + error);
    });
  }

  constructor(private MyDataService: MyDataService) {}

  public initAreaChart(): void {
    this.chartOptions = {
      series: [
        {
          name: '$',
          data: this.dataPoints,
        },
      ],
      chart: {
        type: 'area',
        stacked: false,
        height: 350,
        zoom: {
          type: 'x',
          enabled: true,
          autoScaleYaxis: true,
        },
        toolbar: {
          autoSelected: 'zoom',
        },
      },
      dataLabels: {
        enabled: false,
      },
      markers: {
        size: 0,
      },
      title: {
        text: `${this.tickerTitle}`,
        align: 'center',
      },
      fill: {
        type: 'gradient',
        gradient: {
          shadeIntensity: 1,
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 0.4,
          stops: [0, 100],
        },
      },
      stroke: {
        curve: 'straight',
      },
      yaxis: {
        tooltip: {
          enabled: true,
        },
        title: {
          text: 'Price',
        },
      },
      xaxis: {
        type: 'datetime',
      },
      tooltip: {
        shared: true,
      },
    };
  }

  public initCandleChart(): void {
    this.chartOptions = {
      series: [
        {
          data: this.dataPoints,
        },
      ],
      chart: {
        id: 'chart',
        type: 'candlestick',
        height: 350,
      },
      title: {
        text: `${this.tickerTitle}`,
        align: 'center',
      },
      xaxis: {
        type: 'datetime',
        axisTicks: {
          show: true,
          borderType: 'solid',
          color: '#78909C',
          height: 6,
          offsetX: 0,
          offsetY: 0,
        },
        labels: {
          format: 'MM/yy',
        },
      },
      yaxis: {
        tooltip: {
          enabled: true,
        },
      },
    };
  }

  @ViewChild('chart', { static: false }) chart!: ChartComponent;
  
  public onMatSelectValueChanges(event: any): void {
    if (event.value != 'area') {
      this.initCandleChart();
      this.chart.render();
    } else {
      this.initAreaChart();
      this.chart.render();
    }
  }

  ngAfterViewInit() {
  }
}

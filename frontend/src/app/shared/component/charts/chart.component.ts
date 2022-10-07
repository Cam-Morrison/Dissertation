import { Component, Input, OnChanges, ViewChild } from "@angular/core";
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
    ChartComponent,
    ApexAnnotations
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
     annotations: ApexAnnotations;
     colors: any;
     toolbar: any;
  };

  @Component({
    selector: 'stockChart',
    templateUrl: './chart.component.html',
    styleUrls: ['./chart.component.css']
  })

  export class myChartComponent implements OnChanges {
  
    public chartOptions!: Partial<ChartOptions>;
    today: Date = new Date();
    lastMonthsDate: Date = new Date(this.today.getFullYear(), this.today.getMonth() - 1, this.today.getDate());
    halfYearDate: Date = new Date(this.today.getFullYear(), this.today.getMonth() - 6, this.today.getDate());
    lastYearDate: Date = new Date(this.today.getFullYear() - 1, this.today.getMonth(), this.today.getDate());
    threeYearDate: Date = new Date(this.today.getFullYear() - 3, this.today.getMonth(), this.today.getDate()); 
    @Input() dataPoints: any;
    @Input() ticker?: string;
    @Input() chartType?: string;

    public activeOptionButton = "all";
    public updateOptionsData = {
      "1m": {
        xaxis: {
          min: this.lastMonthsDate.getTime(),
          max: this.today.getTime()
        }
      },
      "6m": {
        xaxis: {
          min: this.halfYearDate.getTime(),
          max: this.today.getTime()
        }
      },
      "1y": {
        xaxis: {
          min: this.lastYearDate.getTime(),
          max: this.today.getTime()
        }
      },
      "3y": {
        xaxis: {
          min: this.threeYearDate.getTime(),
          max: this.today.getTime()
        }
      },
      all: {
        xaxis: {
          min: undefined,
          max: undefined
        }
      }
    };

    public initCandleChart(): void {
        this.chartOptions = {
        series: [ {
          name: '$',
          data: this.dataPoints,
        }
      ],
      chart: {
          id: 'chart',
          type: 'candlestick',
          height: 300,
          zoom: {
            type: 'x',
            enabled: true,
            autoScaleYaxis: true,
          }
      },
      title: {
          text: `${this.ticker}`,
          align: 'center',
      },
      yaxis: {
        tooltip: {
          enabled: false,
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
        height: 300,
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
        text: `${this.ticker}`,
        align: 'center',
      },
      fill: {
        type: 'gradient',
        gradient: {
          shadeIntensity: 1,
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 0.5,
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

  @ViewChild('chart', { static: true }) chart!: ChartComponent;

  ngOnChanges(): void {
      if(this.chartType === "area")
      {
        this.initAreaChart();
      } 
      else 
      {
        this.initCandleChart();
      }
  }

  public updateOptions(option: any): void {
    this.activeOptionButton = option;
    this.chart.updateOptions(this.updateOptionsData["1y"], false, true, true);
  }

}
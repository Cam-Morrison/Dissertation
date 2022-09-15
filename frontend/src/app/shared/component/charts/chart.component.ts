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
    selector: 'stockChart',
    templateUrl: './chart.component.html',
    styleUrls: ['./chart.component.css']
  })

  export class myChartComponent implements OnChanges {
  
    public chartOptions!: Partial<ChartOptions>;
    @Input() dataPoints: any;
    @Input() ticker?: string;
    @Input() chartType?: string;

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
}
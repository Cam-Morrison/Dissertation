import { Component, Input, OnChanges, ViewChild } from "@angular/core";
import {
    ApexAxisChartSeries,
    ApexChart,
    ApexYAxis,
    ApexXAxis,
    ApexTitleSubtitle,
    ApexLegend,
    ApexFill,
    ApexMarkers,
    ApexTooltip,
    ApexDataLabels,
    ApexStroke,
    ChartComponent,
    ApexAnnotations,
    ApexPlotOptions
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
     legend: ApexLegend;
     plotOptions: ApexPlotOptions;
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
        opacity: 1
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

  public initTreemap(): void {
    this.chartOptions = {
      series: [
        {
          data: this.dataPoints,
        }
      ],
      legend: {
        show: false
      },
      chart: {
        height: 350,
        type: "treemap"
      },
      title: {
        text: "Treemap of daily performance",
        align: 'center'
      },
      dataLabels: {
        enabled: true,

        offsetY: -3
      },
      plotOptions: {
        treemap: {
          enableShades: true,
          shadeIntensity: 0.5,
          reverseNegativeShade: true,
          colorScale: {
            ranges: [
              {
                from: -6,
                to: 0,
                color: "#CD363A"
              },
              {
                from: 0.001,
                to: 6,
                color: "#52B12C"
              }
            ]
          }
        }
      }
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
        if(this.chartType === "treemap"){
          this.initTreemap();
        } else{
          this.initCandleChart();
        }
      }
  }
}
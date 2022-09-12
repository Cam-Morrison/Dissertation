// import { Component } from "@angular/core";
// import {
//     ApexAxisChartSeries,
//     ApexChart,
//     ApexYAxis,
//     ApexXAxis,
//     ApexTitleSubtitle,
//     ApexFill,
//     ApexMarkers,
//     ApexTooltip,
//     ApexDataLabels,
//     ApexStroke
//   } from "ng-apexcharts";
  
//   export type ChartOptions = {
//      series: ApexAxisChartSeries;
//      chart: ApexChart;
//      dataLabels: ApexDataLabels;
//      markers: ApexMarkers;
//      title: ApexTitleSubtitle;
//      fill: ApexFill;
//      yaxis: ApexYAxis;
//      xaxis: ApexXAxis;
//      tooltip: ApexTooltip;
//      stroke: ApexStroke;
//   };

//   export class myChartComponent {
  
//     public chartOptions?: Partial<ChartOptions>;

// public initCandleChart(dataPoints: any, ticker: string): any {
//     return this.chartOptions = {
//       series: [
//         {
//           data: dataPoints,
//         },
//       ],
//       chart: {
//         id: 'chart',
//         type: 'candlestick',
//         height: 350,
//       },
//       title: {
//         text: `${ticker}`,
//         align: 'center',
//       },
//       xaxis: {
//         type: 'datetime',
//         axisTicks: {
//           show: true,
//           borderType: 'solid',
//           color: '#78909C',
//           height: 6,
//           offsetX: 0,
//           offsetY: 0,
//         },
//         labels: {
//           format: 'MM/yy',
//         },
//       },
//       yaxis: {
//         tooltip: {
//           enabled: true,
//         },
//       },
//     };
//   }
// }
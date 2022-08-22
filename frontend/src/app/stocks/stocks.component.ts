import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MyDataService } from '../shared/services/data.service';
import { shareReplay } from 'rxjs/operators';
import {MatTableDataSource} from '@angular/material/table';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.css']
})

export class StocksComponent implements OnInit, AfterViewInit {


  public dataPoints: any[] = [];

  displayedColumns: string[] = ['Ticker', 'Volume', 'Open', 'Close', 'Change'];
  dataSource: any;

  isDataAvailable:boolean=false;

  constructor(private MyDataService: MyDataService) {
  }

  ngOnInit(){
    //Read in ticker
    var count = 0;
    let resp = this.MyDataService.getAllStocks().pipe(shareReplay());
    resp.subscribe((data: any)=> {
        for (var key in data['results']) {
          var dt = data['results'][key];
          var percentage = Number(this.getPercentageChange(dt["o"], dt["c"]));
          this.dataPoints.push([dt["T"], dt["v"], dt["o"], dt["c"], percentage]);
          count++;
          if(count === 30){
            break;
          }
        }
        console.log(this.dataPoints);
        this.dataSource = new MatTableDataSource(this.dataPoints);
    },
    (error) => {
      console.log("error is: " + error);
    });
    this.isDataAvailable = true;
  }

  getPercentageChange(oldNumber: number, newNumber: number){
    return (((newNumber - oldNumber)/oldNumber) * 100).toFixed(2);
  }

  ngAfterViewInit() {
  }
}


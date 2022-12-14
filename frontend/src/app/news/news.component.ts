import { Component, OnInit } from '@angular/core';
import { MyDataService } from '../shared/services/data.service';
import { shareReplay } from 'rxjs/operators'

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss']
})
export class NewsComponent implements OnInit {
  sub: any;
  dataPoints: any;
  newsList: any = [];
  isLoading: boolean = true;

  constructor(private MyDataService: MyDataService) { }

  ngOnInit():void {
    let resp = this.MyDataService.getDailyNews().pipe(shareReplay());
    this.sub =  resp.subscribe((data: any)=> {
        for (var key in data) {
          //Gets rid of repetitive photo of bloomberg logo
          if(data[key]["source"] == "Bloomberg")
          {
            data[key]["image"] = null
          }
          this.newsList.push(data[key])
        }
    },   
    (error: any) => {});  
    this.isLoading = false;
  }

}

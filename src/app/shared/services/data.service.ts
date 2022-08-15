import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyDataService {

  private api_key: string = "gnVoGPUtVWs6WuZxrn3sbHcp7RaVn633";
  currentDate = new DatePipe('en-GB').transform(new Date(), 'YYYY-MM-dd');

  constructor(private http: HttpClient){}

  getPrices(ticker: string, from: string){
    return this.http.get(`https://api.polygon.io/v2/aggs/ticker/${ticker}/range/1/day/2018-01-01/${this.currentDate}?adjusted=true&sort=asc&apiKey=${this.api_key}`);
  }
}

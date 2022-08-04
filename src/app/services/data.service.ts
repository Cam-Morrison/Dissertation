import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyDataService {

  private api_key: string = "55FZW4WRIX30CI1R";

  getPrices(ticker: string){
    return this.http.get(`https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=${ticker}&apikey=${this.api_key}`);
  }

  constructor(private http: HttpClient) { }
}

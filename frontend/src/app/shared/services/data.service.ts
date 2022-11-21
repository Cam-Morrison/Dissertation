import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyDataService 
{
  private api_key: string = "gnVoGPUtVWs6WuZxrn3sbHcp7RaVn633";
  private myBackEndService: string = "https://localhost:7299";

  constructor(private http: HttpClient){}

  getStockHistory(ticker: string)
  {
    return this.http.get(`${this.myBackEndService}/history/${ticker}`);
  }

  getStockDetails(ticker: string)
  {
    return this.http.get(`${this.myBackEndService}/details/${ticker}`)
  }

  getAllStocks()
  {
    return this.http.get(`https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/2022-08-17?adjusted=true&include_otc=true&apiKey=${this.api_key}`);
  }
}

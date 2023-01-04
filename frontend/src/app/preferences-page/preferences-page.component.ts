import { Component, OnInit } from '@angular/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';

@Component({
  selector: 'app-preferences-page',
  templateUrl: './preferences-page.component.html',
  styleUrls: ['./preferences-page.component.scss']
})
export class PreferencesPageComponent implements OnInit {

  public AiAssist: any;

  constructor() { }

  ngOnInit(): void {
  }

  public toggle(event: MatSlideToggleChange) {
      console.log('toggle', event.checked);
  }
}

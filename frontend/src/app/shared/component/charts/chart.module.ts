import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { myChartComponent } from './chart.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        NgApexchartsModule,
     ],
    declarations: [
        myChartComponent
    ],
    exports: [
        myChartComponent
    ]
})
export class myChartModule {}
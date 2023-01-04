import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { By } from '@angular/platform-browser';

import { PreferencesPageComponent } from './preferences-page.component';

describe('PreferencesPageComponent', () => {
  let component: PreferencesPageComponent;
  let fixture: ComponentFixture<PreferencesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PreferencesPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PreferencesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call change method on slide change', () => {
    const componentDebug = fixture.debugElement;
    const slider = componentDebug.query(By.directive(MatSlideToggle));
    spyOn(component, 'toggle'); 

    slider.triggerEventHandler('change', null); 

    expect(component.toggle).toHaveBeenCalled(); 
  });
});

import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, Form, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css'
})
export class DatePickerComponent implements ControlValueAccessor{
  @Input() label='';
  @Input() maxDate: Date | undefined;
  bsConfig: Partial<BsDatepickerConfig> | undefined;
  constructor(@Self() public ngControl: NgControl ) {
    
    this.ngControl.valueAccessor = this;
    this.bsConfig={
      containerClass:'theme-red',
      dateInputFormat: 'DD MMMM YYYY'
    }
  }
  

  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
  

}
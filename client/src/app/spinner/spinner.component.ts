import { Component } from '@angular/core';
import { BusyService } from '../_services/busy.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.css'
})
export class SpinnerComponent {
  isBusy = false;

  constructor(private busyService: BusyService) {}

  ngOnInit() {
    this.busyService.busy$.subscribe(busy => {
      this.isBusy = busy;
    });
  }
}

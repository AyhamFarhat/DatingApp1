import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  constructor() { }
  private busySubject = new BehaviorSubject<boolean>(false);
  private busyRequestCount = 0;

  busy$ = this.busySubject.asObservable();
  busy() {
    this.busyRequestCount++;
    if (this.busyRequestCount > 0) {
      this.busySubject.next(true);
    }
  }
  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.busySubject.next(false);
    }
  }
}

import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { delay, finalize, identity, Observable, pipe } from 'rxjs';
import { BusyService } from '../_services/busy.service';
import { environment } from '../../environments/environment';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private busyService: BusyService) { }
  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busyService.busy();
    return next.handle(req).pipe(
       (environment.production ? identity : delay(1000) ) ,
      finalize(() => {
        this.busyService.idle()
      })
    )
  }
  
}

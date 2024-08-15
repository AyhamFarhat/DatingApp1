import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileUploadModule } from 'ng2-file-upload';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({positionClass:'toast-bottom-right'}), // ToastrModule added
    BrowserAnimationsModule,
    FileUploadModule,
    BsDatepickerModule.forRoot(),

  ],
  exports:[
    ToastrModule,
    BrowserAnimationsModule,
    FileUploadModule,
    BsDatepickerModule
  ]
})
export class SharedModule { }

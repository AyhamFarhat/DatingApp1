import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule.forRoot({positionClass:'toast-bottom-right'}), // ToastrModule added
    BrowserAnimationsModule,
    FileUploadModule

  ],
  exports:[
    ToastrModule,
    BrowserAnimationsModule,
    FileUploadModule
  ]
})
export class SharedModule { }

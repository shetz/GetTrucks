import { inject, Injectable } from '@angular/core';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  busyRequestCount=0;
  private spinnerService = inject(NgxSpinnerService);  

  busy(){
    this.busyRequestCount++;
     this.spinnerService.show(undefined,{
       type:'ball-zig-zag-deflect',//'line-scale-pulse-out-rapid',
       bdColor:'rgba(255,255,255,0)',
       color:'#333333'
     })

   // confirm("this is to make it slow");
  }

  idle()
  {
    this.busyRequestCount--;
    if(this.busyRequestCount<=0)
      this.busyRequestCount=0;
    this.spinnerService.hide();
  }
}

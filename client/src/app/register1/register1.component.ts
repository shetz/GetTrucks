import { Component, EventEmitter, inject, Inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register1',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register1.component.html',
  styleUrl: './register1.component.css'
})
export class Register1Component {
  //@Input () usersFromHomeComponent : any;
  //@Output() cancelRegister =new EventEmitter(); 
  cancelRegister = output<boolean>();
  usersFromHomeComponent = input.required<any>()
  model:any={};
  private accountService = Inject(AccountService);
  private toastr = Inject(ToastrService)

  
  register() {
    
     this.accountService.register(this.model).subscribe({
       next:(respone: any)=>{
         console.log(respone);

         this.cancel();
       },
       error: (prog_error: any)=> this.toastr.error(prog_error.error)
     })

  }

  cancel() {
    console.log(this.usersFromHomeComponent());
    console.log("cancelled");
    this.cancelRegister.emit(false)
  }

}

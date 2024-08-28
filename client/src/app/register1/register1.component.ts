import { Component, EventEmitter, Inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

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



  
  register() {
    /*
     console.log(this.model);
      this.accountService.login(this.model).subscribe({
          next: (response: any) =>{
          ///  this.loggedIn=true;
          
            console.log(response);
          },
          error:(error: any)=> console.log(error),
          complete:()=>console.log("req has completed")
        })

this.accountService.login(this.model).subscribe({
          next: (response: any) =>{
          ///  this.loggedIn=true;
          
            console.log(response);
          },
          error:(error: any)=> console.log(error),
          complete:()=>console.log("register has completed")
        })
          */
     this.accountService.register(this.model).subscribe({
       next:(respone: any)=>{
         console.log(respone);

         this.cancel();
       },
       error: (error: any)=> console.log(error)
     })

  }

  cancel() {
    console.log(this.usersFromHomeComponent());
    console.log("cancelled");
    this.cancelRegister.emit(false)
  }

}

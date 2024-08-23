import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { NgIf } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';


@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  private accountService = inject(AccountService);
  model:any={};
  loggedIn:boolean=false;
    login(){
      
      console.log(this.model);
      this.accountService.login(this.model).subscribe({
          next:response=>{
            this.loggedIn=true;
            console.log(response);
          },
          error:error=> console.log(error),
          complete:()=>console.log("req has completed")
        })
    }

    logout()
    {
      this.loggedIn=false;
    }
}

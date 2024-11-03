import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { NgIf, TitleCasePipe } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';


@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule,RouterLink,RouterLinkActive,TitleCasePipe,HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
//[x: string]: string|undefined;
   accountService = inject(AccountService);
    private router = inject(Router);
    private toastr = inject(ToastrService);
    model:any={};
  
    login(){
      
      console.log(this.model);
      this.accountService.login(this.model).subscribe({
          next: (response: any) =>{//next:response=>{
          
          
            console.log(response);
            this.router.navigateByUrl('/members');
          },
          error:prog_error=> {
            this.toastr.error(prog_error.error);
            console.log(prog_error.error);
          },
          complete:()=>console.log("req has completed")
        })
    }

    logout()
    {
      this.accountService.logout(); 
      this.router.navigateByUrl('/');
    }
}

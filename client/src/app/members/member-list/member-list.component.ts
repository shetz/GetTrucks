import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_services/account.service';
import { UserParams } from '../../_models/UserParams';
import { FormsModule, NgForm } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent,PaginationModule,FormsModule,ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{
public memberService = inject(MembersService);
//private accountSerevice = inject(AccountService);
//userParams = new UserParams(this.accountSerevice.currentUser());
genderList=[{value:'male', display:'Males'},{value:'female', display:'Females'}]

members :Member[]=[];


  ngOnInit(): void {

    if(!this.memberService.paginatedResult())
      this.loadMembers();
  
}
resetFilters()
{
  this.memberService.resetUserParams();
  this.loadMembers();
}
loadMembers()
{
 this.memberService.getMembers();
}

  pageChanged(event:any)
  {
    if(this.memberService. userParams().pageNumber!== event.page)
    {
      this.memberService. userParams().pageNumber=event.page;
      this.loadMembers()
    }
  }


}

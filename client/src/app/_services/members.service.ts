import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { AccountService } from './account.service';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
private http=inject(HttpClient);
private accountService=inject(AccountService);

baseUrl= environment.apiUrl;
members = signal<Member[]>([]);
   getMembers()
   {
    return this.http.get<Member[]>(this.baseUrl+'users').subscribe({
      next:server_member =>this.members.set(server_member)
    });
   }

   getMember(username:string)
   {
    const localMember=this.members().find(m=>m.username=== username);
    if(localMember!== undefined) return of(localMember);//observable

    return this.http.get<Member>(this.baseUrl+'users/'+username);
   }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => m.username === member.username 
      //       ? member : m))
      // })
    )
  }

   getHttpOptions()
   {
    return{
      headers: new HttpHeaders({
        Authorization:`Bearer ${this.accountService.currentUser()?.token}`
      })
      
    }
   }
}

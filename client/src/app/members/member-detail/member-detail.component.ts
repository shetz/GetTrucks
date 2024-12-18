
import { Component, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account.service';
import { HubConnectionState } from '@microsoft/signalr';


@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
   this.messageService.stopHubConnection();
  }

ngOnInit(): void {
  
  //the member is loaded by the resolver 
  this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
        this.member && this.member.photos.map(p => {
          this.images.push(new ImageItem({src: p.url, thumb: p.url}))
        })
      }
    })

 this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })

     this.route.paramMap.subscribe({
      next: _ => this.onRouteParamsChange()
    })
    
  }

 @ViewChild('memberTabs',{static:true}) memberTabs?: TabsetComponent;

  activeTab?: TabDirective;
  private messageService = inject(MessageService);
  private memberService = inject(MembersService);
  private accountService = inject(AccountService);
  presenceService = inject(PresenceService);

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  member:Member ={} as Member;
  images:GalleryItem[]=[];

// onUpdateMessages(event:Message)
// {
//   this.messages.push(event);
// }

 selectTab(heading: string) {
  
    if (this.memberTabs) {
      
      const messageTab = this.memberTabs.tabs.find(x => x.heading === heading);
      if (messageTab) messageTab.active = true;
    }
  }

loadMember(){
  const username = this.route.snapshot.paramMap.get('username');
  if(!username)
    return;

  this.memberService.getMember(username).subscribe({
    next: serever_member =>{
    this.member=serever_member;
    serever_member.photos.map(p=>{
      this.images.push(new ImageItem({src:p.url, thumb:p.url}));
    })

    }
  });
}



onTabActivated(data: TabDirective) {
     this.activeTab = data;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {tab: this.activeTab.heading},
      queryParamsHandling: 'merge'
    })
     if (this.activeTab.heading === 'Messages' && this.member ) {
      const user = this.accountService.currentUser();
      if (!user) return;
      this.messageService.createHubConnection(user, this.member.username);
    } else {
      this.messageService.stopHubConnection();

  //   this.messageService.getMessageThread(this.member.username).subscribe({
  //     next:messages => this.messages=messages
  //  })


     }
  }

  onRouteParamsChange() {
    const user = this.accountService.currentUser();
    if (!user) return;
    if (this.messageService.hubConnection?.state === HubConnectionState.Connected && this.activeTab?.heading === 'Messages') {
      this.messageService.hubConnection.stop().then(() => {
        this.messageService.createHubConnection(user, this.member.username);
      })
    }
  }

  }
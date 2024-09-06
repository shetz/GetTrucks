import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';


@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule,GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {

ngOnInit(): void {
  this.loadMember();
}
private memberService = inject(MembersService);
private route = inject(ActivatedRoute);
member?:Member;
images:GalleryItem[]=[];

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

messages(){}
}
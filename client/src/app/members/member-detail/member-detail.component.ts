import { AfterViewInit, Component, OnDestroy, OnInit, viewChild, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgbModule, NgbNav } from '@ng-bootstrap/ng-bootstrap';
import { GalleryComponent, GalleryItem, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account.service';
import { User } from '../../_models/user';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
  imports: [CommonModule, NgbModule, GalleryComponent, TimeagoModule, MemberMessagesComponent,TabsModule ]
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs',{static: true}) memberTabs?: TabsetComponent
  member: Member ={} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[]=[];
  user?: User;
  constructor(private accountService: AccountService, private route: ActivatedRoute,
    private messageService: MessageService, public presenceService: PresenceService,
    private memberService: MembersService, private toastr: ToastrService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user) this.user = user;
        }
      })
    }

 
  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
      next: params =>{
        params['tab'] && this.selectTab(params['tab'])
      }
    })

    this.getImages()
  }

  ngOnDestroy(): void {
      this.messageService.stopHubConnection();
  }

  selectTab(heading: string){
    if(this.memberTabs){
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  onTabActivated(data: TabDirective ){
    this.activeTab =data;
    if(this.activeTab.heading === 'Messages' && this.user){
      //this.loadMessages();
      this.messageService.createHubConnection(this.user, this.member.userName);
    }else{
      this.messageService.stopHubConnection();
    }
  }

  loadMessages(){
    if(this.member){
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  getImages(){
    if(!this.member) return;
    for(const photo of this.member?.photos){
      this.images.push( new ImageItem({ src: photo?.url, thumb: photo?.url }) );


    }
    
  }

  addLike(member: Member){
    this.memberService.addLike(member.userName).subscribe({
      next: () => this.toastr.success('You have liked ' + member.knownAs)
    })
  }

  // setActiveTab(tabId: string ) {
  //   this.activeTabId = tabId;
  // }


}










// import { Component, OnInit } from '@angular/core';
// import { Member } from '../../_models/member';
// import { MembersService } from '../../_services/members.service';
// import { ActivatedRoute } from '@angular/router';
// import { CommonModule } from '@angular/common';
// import {  GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
// import {TabsModule} from 'ngx-bootstrap/tabs';
// @Component({
//   selector: 'app-member-detail',
//   standalone: true,
//   templateUrl: './member-detail.component.html',
//   styleUrl: './member-detail.component.css',
//   imports: [CommonModule, GalleryModule, TabsModule ]
// })
// export class MemberDetailComponent implements OnInit{

//   member: Member | undefined;
//   images: GalleryItem[] = [];
//   // activeTabId: string = '';
//   constructor(private memberService: MembersService, private route: ActivatedRoute) {}

//   ngOnInit(): void {
//     this.loadMember();
//   }

//   loadMember(){
//     const username = this.route.snapshot.paramMap.get('username');
//     if(!username) return;
//     this.memberService.getMember(username).subscribe({
//       next: member => {
//         this.member = member,
//         this.getImages()
//       }
//     })
//   }

//   getImages(){
//     if(!this.member) return;
//     for(const photo of this.member?.photos){
//       this.images.push( new ImageItem({ src: photo?.url, thumb: photo?.url }) );


//     }
    
//   }

//   // setActiveTab(tabId: string ) {
//   //   this.activeTabId = tabId;
//   // }



// }

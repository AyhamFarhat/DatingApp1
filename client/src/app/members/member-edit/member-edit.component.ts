import { ChangeDetectorRef, Component, HostListener, OnInit, ViewChild, viewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;  
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if(this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;

  activeTabId = 1;
  setActiveTab(tabId: number) {
    this.activeTabId = tabId;
  }

  constructor(private accountService: AccountService, private mambersService: MembersService,
     private toastr: ToastrService, private cdr: ChangeDetectorRef) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });
  }

  ngOnInit(): void {
    this.loadMember();

  }

  loadMember() {
    if(!this.user) return;
    this.mambersService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    });
  }
  updateMember() {
    //console.log(this.member);
    this.mambersService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      }
    });
  }

  // check if form is dirty
   checkDirty() {
    if(this.editForm?.dirty) {
      return true;
    }
    return false
  }
}

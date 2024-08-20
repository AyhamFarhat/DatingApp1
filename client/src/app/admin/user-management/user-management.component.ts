import { Component, ModelOptions, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { AdminService } from '../../_services/admin.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';
import { initialState } from 'ngx-bootstrap/timepicker/reducer/timepicker.reducer';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit{
  users: User[] = [];
  bsModelRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  availableRoles = [
    'Admin',
    'Moderator',
    'Member'
  ]

  constructor(private adminService: AdminService, private modaleService: BsModalService) { }

  ngOnInit(): void { this.getUsersWithRoles(); }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next: users => this.users = users
    });
  }

  openRolesModal(user: User){
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        username: user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]

      }
    }
    this.bsModelRef = this.modaleService.show(RolesModalComponent, config);
    this.bsModelRef.onHide?.subscribe({
      next: () => {
        const selectedRoles = this.bsModelRef.content?.selectedRoles;
        if(!this.arrayEqual(selectedRoles!, user.roles)){
          this.adminService.updateUserRoles(user.username, selectedRoles!).subscribe({
            next: roles => user.roles = roles
          })
        }

      }
    })
  }

  private arrayEqual(arr1: any[], arr2: any[]){
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }

}

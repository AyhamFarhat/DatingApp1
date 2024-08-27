// child of member-detail
import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild, viewChild } from '@angular/core';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { CommonModule, NgFor } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
  imports:[CommonModule, TimeagoModule, FormsModule]
})
export class MemberMessagesComponent implements OnInit {
  @Input() username?: string;
  @ViewChild('messageForm') messageForm?: NgForm

  messageContent = '';
  loading = false;

  constructor(public messageService: MessageService) {}
  ngOnInit(): void {

  }
  sendMessage(){
    if(!this.username) return;
    this.loading = true;
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm?.reset();
    }).finally( () => this.loading = false);
  }
}

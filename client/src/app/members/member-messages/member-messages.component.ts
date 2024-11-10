import { AfterViewChecked, Component, ViewChild, inject, input } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements AfterViewChecked {
  @ViewChild('messageForm') messageForm?: NgForm;
  @ViewChild('scrollMe') scrollContainer?: any;
  messageService = inject(MessageService);
  username = input.required<string>();
  messageContent = '';
  loading = false;
  
  sendMessage() {
    this.loading = true;
    this.messageService.sendMessage(this.username(), this.messageContent).then(() => {
      this.messageForm?.reset();
      this.scrollToBottom();
    }).finally(() => this.loading = false);
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    if (this.scrollContainer) {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }
  }
}







// import { AfterViewChecked, Component, ViewChild, inject, input } from '@angular/core';
// import { MessageService } from '../../_services/message.service';
// import { TimeagoModule } from 'ngx-timeago';
// import { FormsModule, NgForm } from '@angular/forms';

// @Component({
//   selector: 'app-member-messages',
//   standalone: true,
//   imports: [TimeagoModule, FormsModule],
//   templateUrl: './member-messages.component.html',
//   styleUrl: './member-messages.component.css'
// })
// export class MemberMessagesComponent implements AfterViewChecked {
//   @ViewChild('messageForm') messageForm?: NgForm;
//   @ViewChild('scrollMe') scrollContainer?: any;
//   messageService = inject(MessageService);
//   username = input.required<string>();
//   messageContent = '';
//   loading = false;
  
//   sendMessage() {
//     this.loading = true;
//     this.messageService.sendMessage(this.username(), this.messageContent).then(() => {
//       this.messageForm?.reset();
//       this.scrollToBottom();
//     }).finally(() => this.loading = false);
//   }



//   ngAfterViewChecked(): void {
//     this.scrollToBottom();
//   }

//   private scrollToBottom() {
//     if (this.scrollContainer) {
//       this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
//     }
//   }
// }



// import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
// import { Message } from '../../_models/message';
// import { MessageService } from '../../_services/message.service';
// import { TimeagoModule } from 'ngx-timeago';
// import { FormsModule, NgForm } from '@angular/forms';

// @Component({
//   selector: 'app-member-messages',
//   standalone: true,
//   imports: [TimeagoModule, FormsModule],
//   templateUrl: './member-messages.component.html',
//   styleUrl: './member-messages.component.css'
// })
// export class MemberMessagesComponent{
//   @ViewChild('messageForm') messageForm? :NgForm;

//   private messageService = inject(MessageService);
//   messageContent ="";
//   username=input.required<string>();

//   messages=input.required<Message[]>();
//   updateMessages =output<Message>();

// sendMessage()
// {
//   this.messageService.sendMessage(this.username(),this.messageContent).subscribe({
//     next: message=>{
//       this.updateMessages.emit(message);
//       this.messageForm?.reset();
//     }
//   })
// }

// }

import { Component, inject, OnInit } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { RegisterComponent } from "../register/register.component";

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [ RegisterComponent]
})
export class HomeComponent implements OnInit {
  ngOnInit(): void {
   
  }
  users:any;
  registerMode = false;
  http = inject(HttpClient);
  registerToggle() {
    this.registerMode = !this.registerMode
  }

  
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  getUsers(){
     this.http.get("https://localhost:5001/api/Users").subscribe({
       next:response=>this.users=response,
       error:error=> console.log(error),
       complete:()=>console.log("req has completed")
    })
  }
}
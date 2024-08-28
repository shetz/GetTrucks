import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { Register1Component } from "../register1/register1.component";
import { HttpClient } from '@angular/common/http';
//import { RegisterComponent } from "../register/register.component";

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [RegisterComponent, Register1Component]
})
export class HomeComponent implements OnInit {
  ngOnInit(): void {
    this.getUsers();
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
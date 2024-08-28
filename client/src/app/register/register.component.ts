import { Component, OnInit, inject, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { NgIf } from '@angular/common';
//import { TextInputComponent } from "../_forms/text-input/text-input.component";
//import { DatePickerComponent } from '../_forms/date-picker/date-picker.component';
import { Router } from '@angular/router';

@Component({
    selector: 'app-register',
    standalone: true,
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    imports: [FormsModule]
})
export class RegisterComponent {
  
  
  

  model:any={};


  register() {
    console.log(this.model);

  }

  cancel() {
    
    console.log("cancelled");
  }

  
}
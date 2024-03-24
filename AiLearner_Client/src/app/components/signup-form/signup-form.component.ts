import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signup-form',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './signup-form.component.html',
  styleUrl: './signup-form.component.css'
})
export class SignupFormComponent {
  userEmail: string = "";
  userPassword: string = "";

  onSubmit(event : Event) {
    event.preventDefault();
    console.log("Email: " + this.userEmail);
    console.log("Password: " + this.userPassword);
  }
}

import { CommonModule } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialService } from '../../../services/material.service';
import { MaterialRequestDTO } from '../../../models/MaterialRequestDTO';
import { UserService } from '../../../services/user.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-new-material',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './new-material.component.html',
  styleUrl: './new-material.component.css',
})
export class NewMaterialComponent implements OnDestroy {
  contentInput: string = '';
  userId: string | null = null;
  private subscription: Subscription;
  loading: boolean = false;

  constructor(
    private materialService: MaterialService,
    private userService: UserService,
    private router : Router,
    private route: ActivatedRoute
  ) {
    this.subscription = this.userService.userId$.subscribe((id) => {
      this.userId = id;
      console.log(this.userId);
    });
  }

  getInput(): void {
    this.loading = true;
    const dto: MaterialRequestDTO = {
      userId: this.userId!,
      content: this.contentInput,
      numOfQuestions: 10,
    };
    this.materialService.registerMaterial(dto).subscribe({
      next: (response) => {
        console.log(response);
        if (response.status === 201) {
          console.log('succssfully created material');
          this.loading = false;
          this.router.navigate(['../materials'], { relativeTo: this.route });
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Error registering user', error);
      },
    });
  }
  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks
    this.subscription.unsubscribe();
  }
}

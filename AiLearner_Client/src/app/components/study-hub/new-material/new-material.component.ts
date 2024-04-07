import { CommonModule } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialService } from '../../../services/material.service';
import { MaterialRequestDTO } from '../../../models/MaterialRequestDTO';
import { UserService } from '../../../services/user.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {
    this.subscription = this.userService.userId$.subscribe({
      next: (id) => {
        if (id) {
          this.userId = id;
        } else {
          this.toastr.info('Try logging in first');
          this.router.navigate(['/login']);
        }
      },
      error: () => {
        this.toastr.info('Try logging in first');
        this.router.navigate(['/login']);
      },
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
          this.loading = false;
          this.toastr.success('Material registered successfully');
          this.router.navigate(['../materials'], { relativeTo: this.route });
        }
      },
      error: () => {
        this.loading = false;
        this.toastr.warning('Error registering material');
      },
    });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}

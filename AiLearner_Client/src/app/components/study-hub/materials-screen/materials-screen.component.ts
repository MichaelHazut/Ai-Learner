import { Component, OnDestroy } from '@angular/core';
import { MaterialDTO } from '../../../models/MaterialDTO';
import { MaterialService } from '../../../services/material.service';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-materials-screen',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './materials-screen.component.html',
  styleUrl: './materials-screen.component.css',
})
export class MaterialsScreenComponent implements OnDestroy {
  userId: string | null = null;
  private subscription: Subscription;

  materials: MaterialDTO[] = [];
  constructor(
    private materialService: MaterialService,
    private userService: UserService
  ) {
    this.subscription = this.userService.userId$.subscribe((id) => {
      this.userId = id;
      console.log(this.userId);
    });
  }

  ngOnInit(): void {
    this.subscription = this.userService.userId$.subscribe((id) => {
      this.userId = id;
      if (this.userId) {
        this.loadMaterials();
      }
    });
  }

  loadMaterials(): void {
    this.materialService.getMaterials(this.userId!).subscribe({
      next: (materials: MaterialDTO[]) => {
        this.materials = materials;
      },
      error: (error) => {
        console.error('There was an error fetching materials!', error);
      },
    });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}

import { Component, OnDestroy } from '@angular/core';
import { MaterialDTO } from '../../../models/MaterialDTO';
import { MaterialService } from '../../../services/material.service';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { Subscription } from 'rxjs';
import { FormatDatePipe } from '../../../pipes/format-date.pipe';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NavigationService } from '../../../services/navigation.service';
@Component({
  selector: 'app-materials-screen',
  standalone: true,
  imports: [CommonModule, FormatDatePipe, RouterLink],
  templateUrl: './materials-screen.component.html',
  styleUrl: './materials-screen.component.css',
})
export class MaterialsScreenComponent {
  userId: string | null = null;
  expandedState: Record<number, boolean> = {};

  materials: MaterialDTO[] | null =  null;

  constructor(
    private materialService: MaterialService,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService,
    private navigationService: NavigationService
  ) {

  }

  ngOnInit(): void {
    this.navigationService.setBackRoute(['/study-hub']);
    this.loadMaterials();
  }
  
  toggleExpanded(materialId: number): void {
    this.expandedState[materialId] = !this.expandedState[materialId];
  }

  loadMaterials(): void {
    this.materialService.getMaterials(this.userId!).subscribe({
      next: (materials: MaterialDTO[]) => {
        this.materials = materials;
      },
      error: (error) => {
        this.toastr.info(`you havent uploaded any materials yet!`);
        this.router.navigate(['/study-hub/new-material']);
      },
    });
  }

  navigateToMaterial(material: MaterialDTO) {
    this.router.navigate(['/study-hub/materials', material.id], { state: { material: material } });
  }

}

import { Component } from '@angular/core';
import { MaterialDTO } from '../../../models/MaterialDTO';
import { MaterialService } from '../../../services/material.service';

@Component({
  selector: 'app-materials-screen',
  standalone: true,
  imports: [],
  templateUrl: './materials-screen.component.html',
  styleUrl: './materials-screen.component.css'
})
export class MaterialsScreenComponent {

    
  constructor(private materialService: MaterialService) {}
  
  ngOnInit(): void {
    const userId = '525c8c8d-a799-439e-9a3a-e8fc1665f923';
    this.materialService.getMaterials(userId).subscribe({
      next: (materials: MaterialDTO[]) => {
        console.log(materials);
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }
}

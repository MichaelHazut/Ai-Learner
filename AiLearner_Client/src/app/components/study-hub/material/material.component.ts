import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MaterialDTO } from '../../../models/MaterialDTO';
import { ContentDialogComponent } from './content-dialog/content-dialog.component';

@Component({
  selector: 'app-material',
  standalone: true,
  imports: [CommonModule, ContentDialogComponent],
  templateUrl: './material.component.html',
  styleUrl: './material.component.css',
})
export class MaterialComponent {
  materialId: string | null = null;
  material: MaterialDTO | null = null;
  showContent: boolean = false;

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit() {
    this.materialId = this.route.snapshot.paramMap.get('id');
    if (window.history.state.material) {
      this.material = window.history.state.material;
    } else {
      // Handle the scenario where materialContent is not available
      console.log('Material content not available');
    }
  }

  showOrHideContent() {
    this.showContent = !this.showContent;
  }
}

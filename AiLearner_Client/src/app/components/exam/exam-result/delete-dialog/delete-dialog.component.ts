import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialService } from '../../../../services/material.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-delete-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.css',
})
export class DeleteDialogComponent {
  deleteVisable: boolean = false;

  @Input() showContent: boolean= false;
  @Output() contentVisibilityChanged: EventEmitter<boolean> = new EventEmitter();


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private materialService: MaterialService,
    private tostar: ToastrService
    ) {}

  
    deleteMaterial() {
    const materialId = parseInt(this.route.snapshot.paramMap.get('id')!);

    this.materialService.deleteMaterial(materialId).subscribe({
      next: (response) => {
        this.tostar.success('Material deleted successfully');
        this.router.navigate(['study-hub/materials']);
      },
      error: (error) => {
      },
    });
  }



  showOrHideContent() {
    this.showContent = !this.showContent;
    this.contentVisibilityChanged.emit(this.showContent);
  }
}

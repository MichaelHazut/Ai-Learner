import { Component, Input } from '@angular/core';
import { MaterialDTO } from '../../../../models/MaterialDTO';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-content-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './content-dialog.component.html',
  styleUrl: './content-dialog.component.css'
})
export class ContentDialogComponent {
  @Input() material: MaterialDTO | null = null;
  @Input() showContent: boolean= false;

  showOrHideContent() {
    this.showContent = !this.showContent;
  }
}
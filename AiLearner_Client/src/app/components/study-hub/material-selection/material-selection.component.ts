import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-material-selection',
  standalone: true,
  imports: [],
  templateUrl: './material-selection.component.html',
  styleUrl: './material-selection.component.css',
})
export class MaterialSelectionComponent {
  constructor(private router: Router, private route: ActivatedRoute) {}

  newMaterial(): void {
    this.router.navigate(['new-material'], { relativeTo: this.route });
  }

  existingMaterial(): void {
    this.router.navigate(['materials'], { relativeTo: this.route });
  }
}

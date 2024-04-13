import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MaterialDTO } from '../../../models/MaterialDTO';
import { ContentDialogComponent } from './content-dialog/content-dialog.component';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../../services/exam-data.service';
import { Exam } from '../../../models/Exam';
import { NavigationService } from '../../../services/navigation.service';

@Component({
  selector: 'app-material',
  standalone: true,
  imports: [CommonModule, RouterLink, ContentDialogComponent],
  templateUrl: './material.component.html',
  styleUrl: './material.component.css',
})
export class MaterialComponent implements OnDestroy {
  material: MaterialDTO | null = null;
  showContent: boolean = false;
  private subsription: Subscription = new Subscription();
  examData: Exam | null = null;

  constructor(
    private examDataService: ExamDataService,
    private route: ActivatedRoute,
    private router: Router,
    private navigationService: NavigationService
  ) {}

  ngOnInit() {
    this.navigationService.setBackRoute(['/study-hub', 'materials']);
    this.subsription = this.examDataService.examData$.subscribe((examData) => {
      this.examData = examData;
      if(examData){
        if (examData.questions.length <= examData.userAnswers.length) {
          this.router.navigate(['./result'], { relativeTo: this.route });
        }
      }
    });
    
    const materialId = this.route.snapshot.paramMap.get('id');
    this.examDataService.getExamData(parseInt(materialId!));
  }

  showOrHideContent() {
    this.showContent = !this.showContent;
  }
  handleVisibilityChange(isVisible: boolean) {
    this.showContent = isVisible;
  }

  ngOnDestroy(){
    this.subsription.unsubscribe();
  }
}

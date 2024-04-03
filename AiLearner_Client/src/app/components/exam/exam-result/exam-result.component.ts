import { Component, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../../services/exam-data.service';
import { Exam } from '../../../models/Exam';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-exam-result',
  standalone: true,
  imports: [],
  templateUrl: './exam-result.component.html',
  styleUrl: './exam-result.component.css',
})
export class ExamResultComponent implements OnDestroy {
  subsription: Subscription = new Subscription();
  @Input() examData: Exam | null = null;

  constructor(
    private examDataService: ExamDataService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.subsription = this.examDataService.examData$.subscribe((examData) => {
      if (!examData) {
        const materialId = this.route.snapshot.paramMap.get('id');
        this.examDataService.getExamData(parseInt(materialId!));
        return;
      }
      this.examData = examData;
    });
  }

  ngOnDestroy(){
    this.subsription.unsubscribe();
  }
}

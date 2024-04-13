import { AfterViewInit, Component, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../../services/exam-data.service';
import { Exam } from '../../../models/Exam';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ContentDialogComponent } from '../../study-hub/material/content-dialog/content-dialog.component';
import { ExamComponent } from '../exam.component';
import { ExamRetryComponent } from '../exam-retry/exam-retry.component';
import { QuestionAndAnswers } from '../../../models/QuestionAndAnswers';
import { MaterialService } from '../../../services/material.service';
import { ToastrService } from 'ngx-toastr';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';
import { NavigationService } from '../../../services/navigation.service';

@Component({
  selector: 'app-exam-result',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ContentDialogComponent,
    ExamRetryComponent,
    DeleteDialogComponent,
  ],
  templateUrl: './exam-result.component.html',
  styleUrl: './exam-result.component.css',
})
export class ExamResultComponent implements OnDestroy, AfterViewInit {
  subsription: Subscription = new Subscription();
  contentVisable: boolean = false;
  deleteVisable: boolean = false;
  answerVisible: boolean = false;
  currectAnswers: any[] = [];
  wrongAnswers: any[] = [];
  animationActive = false;
  private _examData: Exam | null = null;
  @Input()
  set examData(value: Exam | null) {
    this._examData = value;
    if (value) {
      // Trigger the animation once data is available
      this.animationActive = false;
      setTimeout(() => (this.animationActive = true));
    }
  }
  get examData(): Exam | null {
    return this._examData;
  }
  constructor(
    private examDataService: ExamDataService,
    private materialService: MaterialService,
    private route: ActivatedRoute,
    private router: Router,
    private tostar: ToastrService,
    private navigationService: NavigationService
  ) {}

  ngAfterViewInit() {
    setTimeout(() => (this.animationActive = true));
  }

  ngOnInit() {
    this.navigationService.setBackRoute(['/study-hub', 'materials']);
    this.subsription = this.examDataService.examData$.subscribe((examData) => {
      if (!examData) {
        const materialId = this.route.snapshot.paramMap.get('id');
        this.examDataService.getExamData(parseInt(materialId!));
        return;
      }
      this.examData = examData;
      this.getCorrectAnswers();
      this.getIncorrectAnswers();
    });
  }

  getCorrectAnswers() {
    this.currectAnswers = this.examData!.questions.map((question) => ({
      question: question.question,
      answers: question.answers.filter(
        (answer) =>
          answer.isCorrect &&
          this.examData!.userAnswers.some(
            (ua) =>
              ua.answerId === answer.id &&
              ua.questionId === question.question.id
          )
      ),
    })).filter((qa) => qa.answers.length > 0);
  }

  getIncorrectAnswers() {
    this.wrongAnswers = this.examData!.questions.map(
      (question, index): QuestionAndAnswers => ({
        question: question.question,
        answers: question.answers,
        questionIndex: index + 1,
      })
    ).filter((qa) => {
      const wrongAnswers = qa.answers.filter(
        (answer) =>
          !answer.isCorrect &&
          this.examData!.userAnswers.some(
            (ua) =>
              ua.answerId === answer.id && ua.questionId === qa.question.id
          )
      );
      return wrongAnswers.length > 0;
    });
    // filter((qa) => qa.answers.length > 0);
  }

  getCorrectAnswersPercentage(): number {
    if (!this.examData?.questions?.length) {
      return 0;
    }
    const percentage =
      (this.currectAnswers.length / this.examData.questions.length) * 100;
    return Math.round(percentage);
  }

  navigateToExam() {
    this.examDataService.setExamRetryData(this.wrongAnswers);
    this.router.navigate([
      `study-hub/materials/${this.examData?.material.id}/exam/retry/`,
      this.wrongAnswers[0].questionIndex,
    ]);
  }

  showOrHideContent() {
    this.contentVisable = !this.contentVisable;
  }

  handleVisibilityChange(isVisible: boolean) {
    this.contentVisable = isVisible;
  }

  handleDeleteVisibilityChange(isVisible: boolean) {
    this.deleteVisable = isVisible;
  }
  showOrHideDelete() {
    this.deleteVisable = !this.deleteVisable;
  }
  showOrHideAnswers() {
    this.answerVisible = !this.answerVisible;
  }

  ngOnDestroy() {
    this.subsription.unsubscribe();
  }
}

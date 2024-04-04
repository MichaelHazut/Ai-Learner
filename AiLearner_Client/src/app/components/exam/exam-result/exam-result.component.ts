import { Component, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../../services/exam-data.service';
import { Exam } from '../../../models/Exam';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ContentDialogComponent } from '../../study-hub/material/content-dialog/content-dialog.component';
import { ExamComponent } from '../exam.component';
import { ExamRetryComponent } from '../exam-retry/exam-retry.component';
import { QuestionAndAnswers } from '../../../models/QuestionAndAnswers';

@Component({
  selector: 'app-exam-result',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ContentDialogComponent,
    ExamRetryComponent,
  ],
  templateUrl: './exam-result.component.html',
  styleUrl: './exam-result.component.css',
})
export class ExamResultComponent implements OnDestroy {
  subsription: Subscription = new Subscription();
  @Input() examData: Exam | null = null;
  contentVisable: boolean = false;
  answerVisible: boolean = false;
  currectAnswers: any[] = [];
  wrongAnswers: any[] = [];

  constructor(
    private examDataService: ExamDataService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.subsription = this.examDataService.examData$.subscribe((examData) => {
      console.log("in subscription");
      if (!examData) {
        console.log("no exam data fetching...");
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
  showOrHideAnswers() {
    this.answerVisible = !this.answerVisible;
  }

  ngOnDestroy() {
    this.subsription.unsubscribe();
  }
}

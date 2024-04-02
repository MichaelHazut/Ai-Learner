import { Component, OnDestroy } from '@angular/core';
import { QuestionDTO } from '../../models/QuestionDTO';
import { QuestionService } from '../../services/question.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AnswerDTO } from '../../models/AnswerDTO';
import { AnswerService } from '../../services/answer.service';
import { QuestionAndAnswers } from '../../models/QuestionAndAnswers';
import { QuestionComponent } from './question/question.component';
import { Subscription } from 'rxjs';
import { UserAnswersService } from '../../services/user-answers.service';
import { UserAnswersDTO } from '../../models/UserAnswersDTO';
import { UserService } from '../../services/user.service';
import { ExamResultComponent } from './exam-result/exam-result.component';
import { Exam } from '../../models/Exam';
import { ExamDataService } from '../../services/exam-data.service';
@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [CommonModule, QuestionComponent, ExamResultComponent],
  templateUrl: './exam.component.html',
  styleUrl: './exam.component.css',
})
export class ExamComponent implements OnDestroy {
  materialId: string | null = null;
  questionsWithAnswers: QuestionAndAnswers[] = [];
  currentQuestionIndex: number = 0;
  routeSub!: Subscription;
  userId: string | null = null;
  private subscription: Subscription;
  userAnswers: UserAnswersDTO[] = [];

  examData: Exam | null = null;

  constructor(
    private userAnswerService: UserAnswersService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private examService: ExamDataService
  ) {
    this.subscription = this.userService.userId$.subscribe((id) => {
      this.userId = id;
    });
  }

  ngOnInit() {
    this.subscription = this.userService.userId$.subscribe((id) => {
      this.userId = id;
    });
    this.examService.examdata$.subscribe((data) => {
      this.examData = data;
      console.log('new data: ', this.examData);
    });
    this.materialId = this.route.snapshot.paramMap.get('id');

    this.routeSub = this.route.paramMap.subscribe((params) => {
      const index = params.get('questionIndex');
      this.currentQuestionIndex = index ? parseInt(index, 10) : 0;
    });
  }

  handleAnswerSelection(anserId: number): void {
    this.userAnswers.push({
      userId: this.userId!,
      answerId: anserId,
      questionId:
        this.examData?.questions[this.currentQuestionIndex - 1]?.question.id!,
      answerDate: new Date(),
    });
    this.currentQuestionIndex++;

    if (this.currentQuestionIndex <= this.examData?.questions.length!) {
      this.router.navigate([
        `/study-hub/materials/${this.materialId}/exam/${this.currentQuestionIndex}`,
      ]);
    } else {
      this.userAnswerService
        .registerMaterial(this.userAnswers)
        .subscribe((response) => {
          console.log('User answers registered!', response);
        });
      this.router.navigate([
        `/study-hub/materials/${this.materialId}/exam/result`,
      ]);
    }
  }
  ngOnDestroy() {
    this.routeSub.unsubscribe();
    this.subscription.unsubscribe();
  }
}

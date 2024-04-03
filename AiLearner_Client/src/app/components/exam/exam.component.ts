import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { QuestionComponent } from './question/question.component';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../services/exam-data.service';
import { Exam } from '../../models/Exam';
import { UserService } from '../../services/user.service';
import { UserAnswersDTO } from '../../models/UserAnswersDTO';
import { UserAnswersService } from '../../services/user-answers.service';
import { ExamResultComponent } from './exam-result/exam-result.component';
@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [CommonModule, QuestionComponent,ExamResultComponent],
  templateUrl: './exam.component.html',
  styleUrl: './exam.component.css',
})
export class ExamComponent implements OnDestroy {
  materialId: string | null = null;
  currentQuestionIndex: number = 0;
  private subscriptions: Subscription[] = [];
  examData: Exam | null = null;
  userId: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private userAnswersService: UserAnswersService,
    private examDataService: ExamDataService
  ) {}

  ngOnInit() {
    this.materialId = this.route.snapshot.paramMap.get('id');

    this.subscriptions.push(
      this.examDataService.examData$.subscribe((examData) => {
        this.examData = examData;
      })
    );
    this.subscriptions.push(
      this.userService.userId$.subscribe((user) => {
        this.userId = user;
      })
    );
    this.subscriptions.push(
      this.route.paramMap.subscribe((params) => {
        const index = params.get('questionIndex');
        this.currentQuestionIndex = index ? parseInt(index, 10) : 0;
      })
    );
    //this.examDataService.getExamData(parseInt(this.materialId!));
  }

  // loadQuestions(): void {
  //   this.questionsService.getQuestions(this.materialId!).subscribe({
  //     next: (questions: QuestionDTO[]) => {
  //       this.questions = questions;
  //       this.combineQuestionsAndAnswers();
  //     },
  //     error: (error) => {
  //       console.error('There was an error fetching questions!', error);
  //     },
  //   });
  // }
  // loadAnswers(): void {
  //   this.answersService.getAnswers(this.materialId!).subscribe({
  //     next: (answers: AnswerDTO[]) => {
  //       this.answers = answers;
  //       this.combineQuestionsAndAnswers();
  //     },
  //     error: (error) => {
  //       console.error('There was an error fetching questions!', error);
  //     },
  //   });
  // }
  // combineQuestionsAndAnswers(): void {
  //   if (this.questions.length && this.answers.length) {
  //     this.questionsWithAnswers = this.questions.map((question) => ({
  //       question,
  //       answers: this.answers.filter(
  //         (answer) => answer.questionId === question.id
  //       ),
  //     }));
  //   }
  // }
  handleAnswerSelection(anserId: number): void {
    let userAnswer: UserAnswersDTO = {
      questionId:
        this.examData?.questions[this.currentQuestionIndex - 1].question.id!,
      answerId: anserId,
      userId: this.userId!,
      answerDate: new Date(),
    };
    this.examData?.userAnswers.push(userAnswer);

    this.currentQuestionIndex++;
    if (
      this.examData &&
      this.currentQuestionIndex <= this.examData.questions.length
    ) {
      this.router.navigate([
        `/study-hub/materials/${this.materialId}/exam/${this.currentQuestionIndex}`,
      ]);
    } else {
      this.userAnswersService.registerAnswers(this.examData?.userAnswers!).subscribe({
        next: () => {
          this.router.navigate([`/study-hub/materials/${this.materialId}/result`]);
        },
        error: (error) => {
          localStorage.setItem(this.examData?.material.id.toString()!, JSON.stringify(this.examData?.userAnswers));
        },
      });
    }
  }
  ngOnDestroy() {
    this.subscriptions.forEach((subscriptions) => subscriptions.unsubscribe());
  }
}

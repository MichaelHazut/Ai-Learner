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
import { ExamDataService } from '../../services/exam-data.service';
import { Exam } from '../../models/Exam';
@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [CommonModule, QuestionComponent],
  templateUrl: './exam.component.html',
  styleUrl: './exam.component.css',
})
export class ExamComponent implements OnDestroy {
  materialId: string | null = null;
  questions: QuestionDTO[] = [];
  answers: AnswerDTO[] = [];
  questionsWithAnswers: QuestionAndAnswers[] = [];
  currentQuestionIndex: number = 0;
  private subscriptions: Subscription[] = [];
  examData: Exam | null = null;

  constructor(
    private questionsService: QuestionService,
    private answersService: AnswerService,
    private route: ActivatedRoute,
    private router: Router,
    private examDataService: ExamDataService
  ) {}

  ngOnInit() {
    this.materialId = this.route.snapshot.paramMap.get('id');

    // if (this.materialId) {
    //   this.loadQuestions();
    //   this.loadAnswers();
    // }
    this.subscriptions.push(
      this.examDataService.examData$.subscribe((examData) => {
        this.examData = examData;
        console.log('examData: ', examData);
        console.log("length: " , examData?.questions.length);
        console.log("currentQuestionIndex: ", this.currentQuestionIndex);
      })
    );
    this.subscriptions.push(
      this.route.paramMap.subscribe((params) => {
        const index = params.get('questionIndex');
        this.currentQuestionIndex = index ? parseInt(index, 10) : 0;
      })
    );
    this.examDataService.getExamData(parseInt(this.materialId!));
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
    this.currentQuestionIndex++;

    if (
      this.examData &&
      this.currentQuestionIndex <= this.examData.questions.length
    ) {
      this.router.navigate([
        `/study-hub/materials/${this.materialId}/exam/${this.currentQuestionIndex}`,
      ]);
    } else {
      this.router.navigate([`/study-hub/materials/${this.materialId}/result`]);
    }
  }
  ngOnDestroy() {
    this.subscriptions.forEach((subscriptions) => subscriptions.unsubscribe());
  }
}

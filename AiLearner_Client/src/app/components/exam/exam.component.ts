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
  routeSub!: Subscription;

  constructor(
    private questionsService: QuestionService,
    private answersService: AnswerService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.materialId = this.route.snapshot.paramMap.get('id');

    if (this.materialId) {
      this.loadQuestions();
      this.loadAnswers();
    }
    this.routeSub = this.route.paramMap.subscribe((params) => {
      const index = params.get('questionIndex');
      this.currentQuestionIndex = index ? parseInt(index, 10) : 0;
    });
  }

  loadQuestions(): void {
    this.questionsService.getQuestions(this.materialId!).subscribe({
      next: (questions: QuestionDTO[]) => {
        this.questions = questions;
        this.combineQuestionsAndAnswers();
      },
      error: (error) => {
        console.error('There was an error fetching questions!', error);
      },
    });
  }
  loadAnswers(): void {
    this.answersService.getAnswers(this.materialId!).subscribe({
      next: (answers: AnswerDTO[]) => {
        this.answers = answers;
        this.combineQuestionsAndAnswers();
      },
      error: (error) => {
        console.error('There was an error fetching questions!', error);
      },
    });
  }
  combineQuestionsAndAnswers(): void {
    if (this.questions.length && this.answers.length) {
      this.questionsWithAnswers = this.questions.map((question) => ({
        question,
        answers: this.answers.filter(
          (answer) => answer.questionId === question.id
        ),
      }));
    }
  }
  handleAnswerSelection(anserId: number): void {
    this.currentQuestionIndex++;

    if (this.currentQuestionIndex <= this.questionsWithAnswers.length) {
      this.router.navigate([
        `/study-hub/materials/${this.materialId}/exam/${this.currentQuestionIndex}`,
      ]);
    } else {
      this.router.navigate([`/study-hub/materials/${this.materialId}/result`]);
    }
  }
  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }
}

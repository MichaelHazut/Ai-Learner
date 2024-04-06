import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuestionComponent } from '../question/question.component';
import { UserAnswersDTO } from '../../../models/UserAnswersDTO';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionAndAnswers } from '../../../models/QuestionAndAnswers';
import { UserService } from '../../../services/user.service';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../../services/exam-data.service';
import { UserAnswersService } from '../../../services/user-answers.service';
import { Exam } from '../../../models/Exam';

@Component({
  selector: 'app-exam-retry',
  standalone: true,
  imports: [CommonModule, QuestionComponent],
  templateUrl: './exam-retry.component.html',
  styleUrl: './exam-retry.component.css',
})
export class ExamRetryComponent {
  @Input() wrongQuestions: QuestionAndAnswers[] | null = [];
  currentQuestionIndex: number = 0;
  subscriptions: Subscription[] = [];
  newUserAnswers: UserAnswersDTO[] = [];
  userId: string | null = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private examDataService: ExamDataService,
    private userAnswersService: UserAnswersService
  ) {}

  ngOnInit() {
    this.subscriptions.push(
      this.examDataService.examRetryData$.subscribe((examRetryData) => {
        this.wrongQuestions = examRetryData;
      })
    );

    this.subscriptions.push(
      this.userService.userId$.subscribe((user) => {
        this.userId = user;
      })
    );
  }

  handleAnswerSelection(answerId: number): void {
    let userAnswer: UserAnswersDTO = {
      questionId: this.wrongQuestions![this.currentQuestionIndex].question.id!,
      answerId: answerId,
      userId: this.userId!,
      answerDate: new Date(),
    };
    this.newUserAnswers.push(userAnswer);

    this.currentQuestionIndex++;
    const materialId = this.wrongQuestions![0].question.materialId;
    if (
      this.wrongQuestions &&
      this.currentQuestionIndex < this.wrongQuestions.length
    ) {
      const questionIndex =
        this.wrongQuestions[this.currentQuestionIndex].questionIndex;
      this.router.navigate([
        `/study-hub/materials/${materialId}/exam/retry/${questionIndex}`,
      ]);
    } else {
      this.userAnswersService
        .updateUserAnswers(
          this.wrongQuestions![0].question.materialId,
          this.newUserAnswers
        )
        .subscribe({
          next: () => {
            this.examDataService.fetchExamData(materialId).subscribe({
              next: ({ material, questions, answers, userAnswers }) => {
                let examObj = new Exam(
                  material,
                  questions,
                  answers,
                  userAnswers
                );
                this.examDataService.examData.next(examObj);
                this.router.navigate([
                  `/study-hub/materials/${materialId}/result`,
                ]);
              },
              error: (error) => console.error('Error fetching data:', error),
            });
          },
          error: (error) => {
            console.error(
              'Failed to register answers, saving to local storage',
              error
            );
            localStorage.setItem(
              `New_Answers_${materialId.toString()}`,
              JSON.stringify(this.newUserAnswers)
            );
            this.router.navigate([`/study-hub/materials`]);
          },
        });
    }
  }
}

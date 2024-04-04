import { Component, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart, Event as RouterEvent } from '@angular/router';
import { CommonModule } from '@angular/common';
import { QuestionComponent } from './question/question.component';
import { Subscription, filter } from 'rxjs';
import { ExamDataService } from '../../services/exam-data.service';
import { Exam } from '../../models/Exam';
import { UserService } from '../../services/user.service';
import { UserAnswersDTO } from '../../models/UserAnswersDTO';
import { UserAnswersService } from '../../services/user-answers.service';
import { ExamResultComponent } from './exam-result/exam-result.component';
@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [CommonModule, QuestionComponent, ExamResultComponent],
  templateUrl: './exam.component.html',
  styleUrl: './exam.component.css',
})
export class ExamComponent implements OnDestroy {
  materialId: string | null = null;
  currentQuestionIndex: number = 0;
  private subscriptions: Subscription[] = [];
  @Input() examData: Exam | null = null;
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

    this.router.events.pipe(
      filter((event: RouterEvent): event is NavigationStart => event instanceof NavigationStart)
    ).subscribe((event: NavigationStart) => {
      // Now 'event' is assured to be of type NavigationStart
      const tryData = history.state?.wrongAnswers
      if(tryData){
        this.examData = history.state?.wrongAnswers;
       }
    });
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
  }

  handleAnswerSelection(answerId: number): void {
    let userAnswer: UserAnswersDTO = {
      questionId:
        this.examData?.questions[this.currentQuestionIndex - 1].question.id!,
      answerId: answerId,
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
      this.userAnswersService
        .registerAnswers(
          this.examData?.material.id!,
          this.examData?.userAnswers!
        )
        .subscribe({
          next: () => {
            this.router.navigate([
              `/study-hub/materials/${this.materialId}/result`,
            ]);
          },
          error: (error) => {
            // If there is an error, log it and save the answers in local storage
            console.error(
              'Failed to register answers, saving to local storage',
              error
            );
            localStorage.setItem(
              this.examData?.material.id.toString()!,
              JSON.stringify(this.examData?.userAnswers)
            );
            this.router.navigate([`/study-hub/materials`]);
          },
        });
    }
  }
  ngOnDestroy() {
    this.subscriptions.forEach((subscriptions) => subscriptions.unsubscribe());
  }
}

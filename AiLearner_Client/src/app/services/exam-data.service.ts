import { Injectable } from '@angular/core';
import { MaterialService } from './material.service';
import { QuestionService } from './question.service';
import { AnswerService } from './answer.service';
import { UserAnswersService } from './user-answers.service';
import { Observable, forkJoin, switchMap } from 'rxjs';
import { MaterialDTO } from '../models/MaterialDTO';
import { QuestionDTO } from '../models/QuestionDTO';
import { AnswerDTO } from '../models/AnswerDTO';
import { UserAnswersDTO } from '../models/UserAnswersDTO';

@Injectable({
  providedIn: 'root',
})
export class ExamDataService {
  constructor(
    private materialService: MaterialService,
    private questionService: QuestionService,
    private answerService: AnswerService,
    private userAnswersService: UserAnswersService
  ) {}

  fetchExamData(materialId: number): Observable<{ material: MaterialDTO; questions: QuestionDTO[]; answers: AnswerDTO[]; userAnswers: UserAnswersDTO[]; }> {
    return forkJoin({
      material: this.materialService.getMaterialById(materialId),
      questions: this.questionService.getQuestions(materialId),
      answers: this.answerService.getAnswers(materialId),
      userAnswers: this.userAnswersService.getUserAnswers(materialId)
    });
  }

  logData(materialId: number): void {
    this.fetchExamData(materialId).subscribe({
      next: ({ material, questions, answers, userAnswers }) => {
        console.log('Material:', material);
        console.log('Questions:', questions);
        console.log('Answers:', answers);
        console.log('User Answers:', userAnswers);
      },
      error: (error) => console.error('Error fetching data:', error),
      complete: () => console.log('Data fetching complete.')
    });
  }
}

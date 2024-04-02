import { Injectable } from '@angular/core';
import { MaterialService } from './material.service';
import { QuestionService } from './question.service';
import { AnswerService } from './answer.service';
import { UserAnswersService } from './user-answers.service';
import { BehaviorSubject, Observable, forkJoin, switchMap } from 'rxjs';
import { MaterialDTO } from '../models/MaterialDTO';
import { QuestionDTO } from '../models/QuestionDTO';
import { AnswerDTO } from '../models/AnswerDTO';
import { UserAnswersDTO } from '../models/UserAnswersDTO';
import { Exam } from '../models/Exam';

@Injectable({
  providedIn: 'root',
})
export class ExamDataService {
  private examData = new BehaviorSubject<Exam | null>(null);
  examData$ = this.examData.asObservable();

  constructor(
    private materialService: MaterialService,
    private questionService: QuestionService,
    private answerService: AnswerService,
    private userAnswersService: UserAnswersService
  ) {}

  fetchExamData(
    materialId: number
  ): Observable<{
    material: MaterialDTO;
    questions: QuestionDTO[];
    answers: AnswerDTO[];
    userAnswers: UserAnswersDTO[];
  }> {
    return forkJoin({
      material: this.materialService.getMaterialById(materialId),
      questions: this.questionService.getQuestions(materialId),
      answers: this.answerService.getAnswers(materialId),
      userAnswers: this.userAnswersService.getUserAnswers(materialId),
    });
  }

  getExamData(materialId: number): void {
    this.fetchExamData(materialId).subscribe({
      next: ({ material, questions, answers, userAnswers }) => {
        let examObj = new Exam(material, questions, answers, userAnswers);
        this.examData.next(examObj);
      },
      error: (error) => console.error('Error fetching data:', error),
      complete: () => console.log('Data fetching complete.'),
    });
  }
}

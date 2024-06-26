import { Injectable } from '@angular/core';
import { MaterialService } from './material.service';
import { QuestionService } from './question.service';
import { AnswerService } from './answer.service';
import { UserAnswersService } from './user-answers.service';
import { RecommendationService } from './recommendation.service';
import { BehaviorSubject, Observable, forkJoin, switchMap } from 'rxjs';
import { MaterialDTO } from '../models/MaterialDTO';
import { QuestionDTO } from '../models/QuestionDTO';
import { AnswerDTO } from '../models/AnswerDTO';
import { UserAnswersDTO } from '../models/UserAnswersDTO';
import { RecommendationDTO } from '../models/RecommendationDTO';
import { Exam } from '../models/Exam';
import { QuestionAndAnswers } from '../models/QuestionAndAnswers';

@Injectable({
  providedIn: 'root',
})
export class ExamDataService {
  examData = new BehaviorSubject<Exam | null>(null);
  examData$ = this.examData.asObservable();
  
  private examRetryData = new BehaviorSubject<QuestionAndAnswers[]| null>(null);
  examRetryData$ = this.examRetryData.asObservable();

  constructor(
    private materialService: MaterialService,
    private questionService: QuestionService,
    private answerService: AnswerService,
    private userAnswersService: UserAnswersService,
    private RecommendationService : RecommendationService
  ) {}

  fetchExamData(
    materialId: number
  ): Observable<{
    material: MaterialDTO;
    questions: QuestionDTO[];
    answers: AnswerDTO[];
    userAnswers: UserAnswersDTO[];
    recommendations: RecommendationDTO[];
  }> {
    return forkJoin({
      material: this.materialService.getMaterialById(materialId),
      questions: this.questionService.getQuestions(materialId),
      answers: this.answerService.getAnswers(materialId),
      userAnswers: this.userAnswersService.getUserAnswers(materialId),
      recommendations: this.RecommendationService.getRecommendations(materialId)
    });
  }

  getExamData(materialId: number): void {
    const currentExamData = this.examData.value;
    if(currentExamData && currentExamData.material.id === materialId) {
      return;
    }
    this.fetchExamData(materialId).subscribe({
      next: ({ material, questions, answers, userAnswers, recommendations }) => {
        let examObj = new Exam(material, questions, answers, userAnswers, recommendations);
        this.examData.next(examObj);
      },
      error: (error) => console.error('Error fetching data:', error)
    });
  }

  setExamRetryData(wrongQuestions: QuestionAndAnswers[]): void {
    this.examRetryData.next(wrongQuestions);
  }
}

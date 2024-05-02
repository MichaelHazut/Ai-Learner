import { MaterialDTO } from './MaterialDTO';
import { AnswerDTO } from './AnswerDTO';
import { UserAnswersDTO } from './UserAnswersDTO';
import { QuestionAndAnswers } from './QuestionAndAnswers';
import { QuestionDTO } from './QuestionDTO';
import { RecommendationDTO } from './RecommendationDTO';

export class Exam {
  material: MaterialDTO;
  questions: QuestionAndAnswers[];
  userAnswers: UserAnswersDTO[] = [];
  recommendations : RecommendationDTO[] = [];
  
  constructor(
    material: MaterialDTO,
    questions: QuestionDTO[] = [],
    answers: AnswerDTO[] = [],
    userAnswers: UserAnswersDTO[],
    recommendation: RecommendationDTO[]
  ) {
    this.material = material;
    this.userAnswers = userAnswers;
    this.recommendations = recommendation;
    
    this.questions = questions.map(question => ({
      question,
      answers: answers.filter(answer => answer.questionId === question.id),
    }));
  }

  getCurrectAnswers(): AnswerDTO[] {
    let correctUserAnswers: AnswerDTO[] = [];
    this.questions.forEach((question) => {
      question.answers.forEach((answer) => {
        if (answer.isCorrect) {
          const userAnswers = this.userAnswers.find(
            (ua) =>
              ua.answerId === answer.id &&
              ua.questionId === question.question.id
          );
          if (userAnswers) {
            correctUserAnswers.push(answer);
          }
        }
      });
    });

    return correctUserAnswers;

  }
}

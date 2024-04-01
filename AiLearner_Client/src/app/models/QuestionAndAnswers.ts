import { QuestionDTO } from "./QuestionDTO";
import { AnswerDTO } from "./AnswerDTO";

export interface QuestionAndAnswers {
    question: QuestionDTO;
    answers: AnswerDTO[];
}
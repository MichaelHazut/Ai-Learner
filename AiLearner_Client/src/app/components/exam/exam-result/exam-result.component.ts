import { Component, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ExamDataService } from '../../../services/exam-data.service';
import { Exam } from '../../../models/Exam';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ContentDialogComponent } from '../../study-hub/material/content-dialog/content-dialog.component';

@Component({
  selector: 'app-exam-result',
  standalone: true,
  imports: [CommonModule, ContentDialogComponent ],
  templateUrl: './exam-result.component.html',
  styleUrl: './exam-result.component.css',
})
export class ExamResultComponent implements OnDestroy {
  subsription: Subscription = new Subscription();
  @Input() examData: Exam | null = null;
  contentVisable : boolean = false;
  answerVisable : boolean = false;
  currectAnswers: any[] = [];
  wrongAnswers: any[] = [];

  constructor(
    private examDataService: ExamDataService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.subsription = this.examDataService.examData$.subscribe((examData) => {
      if (!examData) {
        const materialId = this.route.snapshot.paramMap.get('id');
        this.examDataService.getExamData(parseInt(materialId!));
        return;
      }
      this.examData = examData;
      console.log("examData: ", this.examData);
      this.getCorrectAnswers();
      this.getIncorrectAnswers();
    });
  }

  getCorrectAnswers() {
    this.currectAnswers = this.examData!.questions.map(question => ({
      question: question.question,
      answers: question.answers.filter(answer =>
        answer.isCorrect && 
        this.examData!.userAnswers.some(ua => ua.answerId === answer.id && ua.questionId === question.question.id)
      )
    })).filter(qa => qa.answers.length > 0);
  }

  getIncorrectAnswers() {
    this.wrongAnswers =  this.examData!.questions.map(question => ({
      question: question.question,
      answers: question.answers.filter(answer =>
        !answer.isCorrect && 
        this.examData!.userAnswers.some(ua => ua.answerId === answer.id && ua.questionId === question.question.id)
      )
    })).filter(qa => qa.answers.length > 0);
  }

  showOrHideContent(){
    this.contentVisable = !this.contentVisable;
  }
  showOrHideAnswers(){
    this.answerVisable = !this.answerVisable;
  }

  ngOnDestroy(){
    this.subsription.unsubscribe();
  }
}

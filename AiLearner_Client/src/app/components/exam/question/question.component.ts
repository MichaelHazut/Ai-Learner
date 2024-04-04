import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, Output, input } from '@angular/core';
import { common } from '@mui/material/colors';
import { QuestionDTO } from '../../../models/QuestionDTO';
import { AnswerDTO } from '../../../models/AnswerDTO';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './question.component.html',
  styleUrl: './question.component.css',
})
export class QuestionComponent implements OnDestroy{
  @Input() question!: QuestionDTO;
  @Input() answers!: AnswerDTO[];
  @Output() answerSelected = new EventEmitter<number>();
  
  routeSub!: Subscription;
  questionNumber: string | null = '';

  constructor(private route: ActivatedRoute) {}
  ngOnInit() {
    this.questionNumber = this.route.snapshot.paramMap.get('questionIndex') || '';
    this.routeSub = this.route.paramMap.subscribe(params => {
      this.questionNumber = params.get('questionIndex');
    });
  }
  selectAnswer(answerId: number) {
    this.answerSelected.emit(answerId);
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }
}

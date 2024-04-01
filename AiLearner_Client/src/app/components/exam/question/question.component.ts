import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, input } from '@angular/core';
import { common } from '@mui/material/colors';
import { QuestionDTO } from '../../../models/QuestionDTO';
import { AnswerDTO } from '../../../models/AnswerDTO';

@Component({
  selector: 'app-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './question.component.html',
  styleUrl: './question.component.css',
})
export class QuestionComponent {
  @Input() question!: QuestionDTO;
  @Input() answers!: AnswerDTO[];

  @Output() answerSelected = new EventEmitter<number>();
  ngOnInit() {
    console.log('Question:', this.question);
    console.log('Answers:', this.answers);
  }
  selectAnswer(answerId: number) {
    this.answerSelected.emit(answerId);
  }
}

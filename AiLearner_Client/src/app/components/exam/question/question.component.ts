import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  input,
} from '@angular/core';
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
export class QuestionComponent implements OnDestroy {
  @Input() question!: QuestionDTO;
  @Input() answers!: AnswerDTO[];
  @Output() answerSelected = new EventEmitter<number>();
  routeSub!: Subscription;
  questionNumber: string | null = '';
  animationActive = false;

  constructor(
    private route: ActivatedRoute,
    private changeDetector: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.toggleAnimation();

    this.routeSub = this.route.paramMap.subscribe((params) => {
      this.questionNumber = params.get('questionIndex');
      this.toggleAnimation();
    });
  }

  toggleAnimation() {
    this.animationActive = false;
    this.changeDetector.detectChanges();
    setTimeout(() => {
      this.animationActive = true;
      this.changeDetector.detectChanges();
    });
  }
  selectAnswer(answerId: number) {
    this.answerSelected.emit(answerId);
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }
}

import { Component } from '@angular/core';
import { QuestionDTO } from '../../models/QuestionDTO';
import { QuestionService } from '../../services/question.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [],
  templateUrl: './exam.component.html',
  styleUrl: './exam.component.css'
})
export class ExamComponent {
  materialId: string | null = null;
  materialContent: string = '';
  questions: QuestionDTO[] = [];

  constructor(
    private questionsService: QuestionService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.materialId = this.route.snapshot.paramMap.get('id');
    const navigation = this.router.getCurrentNavigation();
    this.materialContent = navigation?.extras.state?.['materialContent'];
    console.log(this.materialId);
    if (this.materialId) {
      this.loadQuestions();
    }
  }

  loadQuestions(): void {
    this.questionsService.getQuestions(this.materialId!).subscribe({
      next: (questions: QuestionDTO[]) => {
        this.questions = questions;
      },
      error: (error) => {
        console.error('There was an error fetching questions!', error);
      },
    });
  }
}
